using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BibleReader.utils {
    public static class EnumUtils {
        // http://stackoverflow.com/questions/588643/generic-method-with-multiple-constraints
        public static string GetDescription<T>(this T enumerationValue) where T : IConvertible {
            Type type = enumerationValue.GetType();
            MemberInfo[] memberInfo = type.GetMember(enumerationValue.ToString());
            if (memberInfo != null && memberInfo.Length > 0) {
                object[] attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs != null && attrs.Length > 0)
                    return ((DescriptionAttribute)attrs[0]).Description;
            }

            return enumerationValue.ToString();
        }

        public static T[] List<T>(params T[] excludes) {
            List<T> values = typeof(T).GetEnumValues().Cast<T>().ToList();
            foreach (T exclude in excludes)
                values.Remove(exclude);

            return values.ToArray();
        }

        public static T Parse<T>(string text, bool caseSensitive = false) {
            return (T)Parse(typeof(T), text, caseSensitive);
        }

        public static object Parse(Type t, string text, bool caseSensitive = false) {
            text = text.Trim().Replace(" ", "");

            foreach (object item in Enum.GetValues(t)) {
                if (caseSensitive) {
                    if (text == item.ToString())
                        return item;
                } else {
                    if (text.ToLower() == item.ToString().ToLower())
                        return item;
                }
            }

            throw new Exception(string.Format("Could not parse '{0}' as type {1}. Available values are: {2}", text, t.Name, string.Join(", ", Enum.GetValues(t))));
        }
    }
}
