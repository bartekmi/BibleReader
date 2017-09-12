using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BibleReader.utils;
using BibleReader.model;

namespace BibleReader.apis {
    public class EsvApi : RestApi {

        private const string KEY = "IP";

        public EsvApi() {
            ApiBaseUrl = "http://www.esvapi.org/v2/rest/";
        }

        public string[] GetChapter(Chapter chapter) {
            string passage = string.Format("{0}+{1}", chapter.Book.Name, chapter.Number);
            string chapterText = Get("passageQuery",
                new UriParam("output-format", "plain-text"),
                new UriParam("passage", passage),
                new UriParam("include-passage-references", false),
                //new UriParam("include-first-verse-numbers", false),
                new UriParam("include-verse-numbers", true),
                new UriParam("include-footnotes", false),
                new UriParam("include-short-copyright", false),
                new UriParam("include-passage-horizontal-lines", false),
                new UriParam("include-headings", false),
                new UriParam("include-subheadings", false),
                new UriParam("line-length", 0)          // Unlimited... One line per paragraph
                );

            if (chapterText == null)
                return null;

            string[] verses = ParseVerses(chapterText, chapter);
            return verses;
        }

        private enum State {
            Start,
            InBracket,
            InVerse
        }

        private string[] ParseVerses(string chapterText, Chapter chapter) {
            State state = State.Start;
            StringBuilder bracketed = new StringBuilder();
            StringBuilder verse = new StringBuilder();
            int expectedVerseNumber = 1;
            List<string> verses = new List<string>();

            foreach (char c in chapterText) {
                if (c == '[') {
                    state = State.InBracket;
                } else if (c == ']') {
                    bool isFalseAlarm = false;

                    if (expectedVerseNumber > 1) {
                        int actualVerseNumber;
                        if (int.TryParse(bracketed.ToString(), out actualVerseNumber)) {
                            if (actualVerseNumber != expectedVerseNumber)
                                throw new Exception(string.Format("Verse number mismatch: Book {0}; Chapter {1}; Expected Verse Number {2} Actual {3}",
                                    chapter.Book, chapter.Number, expectedVerseNumber, actualVerseNumber));
                            verses.Add(MassageVerse(verse));

                        } else {
                            // Sometimes, we get [] brackets in the text itself
                            verse.Append('[');
                            verse.Append(bracketed.ToString());
                            verse.Append(']');
                            isFalseAlarm = true;
                        }
                    }

                    if (!isFalseAlarm) {
                        expectedVerseNumber++;
                        verse.Clear();
                    }
                    bracketed.Clear();
                    state = State.InVerse;
                } else                                      // Any other character
                    switch (state) {
                        case State.Start:
                            break;
                        case State.InBracket:
                            bracketed.Append(c);
                            break;
                        case State.InVerse:
                            verse.Append(c);
                            break;

                    }
            }

            verses.Add(MassageVerse(verse));

            return verses.ToArray();
        }

        private string MassageVerse(StringBuilder builder) {
            return builder.ToString().Trim() + " (ESV)";
        }

        protected override Uri BuildQueryUrl(string relativeUrl, UriParam[] uriParams, bool queryParamsAsQuestionMark, bool isForCache) {
            List<UriParam> list = uriParams.ToList();
            list.Add(new UriParam("key", KEY));

            return base.BuildQueryUrl(relativeUrl, list.ToArray(), true, isForCache);
        }
    }

}
