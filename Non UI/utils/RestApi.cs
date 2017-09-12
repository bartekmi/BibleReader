using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using System.Collections.Specialized;
using Newtonsoft.Json.Linq;
using System.Web;
using System.IO;

namespace BibleReader.utils {

    public class RestApi {

        public string ApiBaseUrl { get; set; }

        public OUT Post<IN, OUT>(string relativeUrl, IN input) {
            string payload = JsonConvert.SerializeObject(input);
            return MakeTheCall<OUT>(relativeUrl, true, new UriParam[0], (client, uri) => client.UploadString(uri, "POST", payload));
        }

        public string Get(string relativeUrl, params UriParam[] uriParams) {
            return Get<string>(relativeUrl, uriParams);
        }

        public OUT Get<OUT>(string relativeUrl, params UriParam[] uriParams) {
            return Get<OUT>(relativeUrl, true, uriParams);
        }

        public OUT Get<OUT>(string relativeUrl, bool queryParamsAsQuestionMark, params UriParam[] uriParams) {
            return MakeTheCall<OUT>(relativeUrl, queryParamsAsQuestionMark, uriParams, (client, uri) => client.DownloadString(uri));
        }

        private OUT MakeTheCall<OUT>(string relativeUrl, bool queryParamsAsQuestionMark, UriParam[] uriParams, Func<WebClient, Uri, string> theCall) {
            Uri uriForCache = BuildQueryUrl(relativeUrl, uriParams, queryParamsAsQuestionMark, true);

            DateTime start = DateTime.Now;
            string responseText = null;

            Uri uri = BuildQueryUrl(relativeUrl, uriParams, queryParamsAsQuestionMark, false);
            using (WebClient client = GetClient())
                try {
                    responseText = theCall(client, uri);
                } catch {
                    return default(OUT);
                }

            TimeSpan duration = DateTime.Now - start;
            OUT response = Deserialize<OUT>(responseText);

            RecordForPosterity(uriForCache, responseText, duration);

            return response;
        }

        public static OUT Deserialize<OUT>(string json) {
            JsonSerializerSettings settings = new JsonSerializerSettings {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
            };

            OUT result;
            if (typeof(OUT) == typeof(string))
                result = (OUT)(object)json;
            else {
                result = JsonConvert.DeserializeObject<OUT>(json, settings);

                PostProcess(result);
                if (result is IEnumerable)
                    foreach (object item in (result as IEnumerable))
                        PostProcess(item);
            }

            return result;
        }

        private static void PostProcess(object item) {
            // Implement when needed
        }

        protected virtual void RecordForPosterity(Uri uri, string text, TimeSpan duration) {
            Console.WriteLine(string.Format("{0}\r\n{1}\r\nTime Taken: {2}", uri, text, duration));
        }

        protected virtual WebClient GetClient() {
            WebClient client = new GZipWebClient();
            client.Headers.Add("content-type", "application/json");
            return client;
        }

        protected virtual Uri BuildQueryUrl(string relativeUrl, UriParam[] uriParams, bool queryParamsAsQuestionMark, bool isForCache) {

            string parametersText = "";

            if (queryParamsAsQuestionMark) {
                NameValueCollection parameters = HttpUtility.ParseQueryString(string.Empty);
                foreach (UriParam parm in uriParams)
                    if (parm != null && parm.Value != null)
                        parameters[parm.Name] = parm.Value;
                parametersText = "?" + parameters;
            } else {
                StringBuilder builder = new StringBuilder();

                foreach (UriParam p in uriParams) {
                    builder.Append('/');
                    builder.Append(p.Value);
                }

                parametersText = builder.ToString();
            }

            bool isAbsolute = relativeUrl.Contains(':');
            string url = string.Format("{0}{1}{2}", isAbsolute ? "" : ApiBaseUrl, relativeUrl, parametersText);
            return new Uri(url);
        }
    }

    #region Helper Classes and Enums

    // Source - http://stackoverflow.com/questions/24613769/downloadstring-and-httpwebresponse-is-not-returning-the-full-json-content
    public class GZipWebClient : WebClient {
        protected override WebRequest GetWebRequest(Uri address) {
            HttpWebRequest request = (HttpWebRequest)base.GetWebRequest(address);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            return request;
        }
    }

    public enum UriParamDateFormat {
        UnixEpoch,
        ISODate,
    }

    public class UriParam {
        internal string Name { get; set; }
        internal string Value { get; set; }

        private object _valueObject;
        internal Func<object, string, bool> StringFilter { get; private set; }
        internal Func<object, bool, bool> BoolFilter { get; private set; }
        internal Func<object, DateTime, bool> DateTimeFilter { get; private set; }

        public UriParam(string name, string value, Func<object, string, bool> filter = null) {
            Name = name;
            Value = value;
            _valueObject = value;
            StringFilter = filter;
        }

        public UriParam(string name, Enum value) {
            Name = name;
            if (value != null)
                Value = value.ToString().ToLower();
        }

        public UriParam(string name, object value) {
            Name = name;
            Value = value.ToString();
        }

        public UriParam(string name, bool value, Func<object, bool, bool> filter = null) {
            Name = name;
            Value = value ? "true" : "false";
            _valueObject = value;
            BoolFilter = filter;
        }
    }
    #endregion
}
