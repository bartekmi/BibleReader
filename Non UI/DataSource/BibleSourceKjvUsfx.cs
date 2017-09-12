using BibleReader.model;
using BibleReader.model.enums;
using System.Linq;
using System.Xml;
using System;

namespace BibleReader.DataSource {
    public class BibleSourceKjvUsfx : BibleSource {
        private const string FILENAME = @"Data\KjvUSFX.EBible.org.xml";

        public override Bible HydrateBible() {
            XmlDocument doc = new XmlDocument();
            doc.Load(FILENAME);
            XmlNode root = doc.ChildNodes[1];

            Bible bible = new Bible() {
                Name = "King James Version",
                Language = Language.English,
            };

            foreach (XmlNode child in root.ChildNodes)
                if (child.Name == "book") {
                    if (GetAttribute(child, "id") == "FRT") {
                        // Ignore the Preface
                    } else
                        bible.Books.Add(ReadBook(bible, child));
                }

            return bible;
        }

        private Book ReadBook(Bible bible, XmlNode node) {
            string abbreviation = GetAttribute(node, "id");

            Book book = new Book(bible) {
                Abbreviation = abbreviation,
                BookEnum = ToEnum(abbreviation),
            };

            Chapter chapter = null;
            foreach (XmlNode child in node.ChildNodes)
                switch (child.Name) {
                    case "h":
                        book.Name = child.InnerText.Trim();
                        break;
                    case "c":
                        chapter = new Chapter(book) {
                            Number = GetAttributeInt(child, "id"),
                        };
                        book.Chapters.Add(chapter);
                        break;
                    case "p":
                    case "q":       // For some reason, Psalms have q instead of p
                        ReadParagraph(chapter, child);
                        break;
                }

            return book;
        }

        private BookEnum ToEnum(string abbreviation) {
            foreach (BookEnum value in Enum.GetValues(typeof(BookEnum))) 
                if (value.ToString().Replace("_", "").ToLower() == abbreviation.ToLower())
                    return value;

            throw new Exception("Unknown book abbreviation: " + abbreviation);
        }

        private void ReadParagraph(Chapter chapter, XmlNode node) {
            Verse verse = null;
            bool isNewParagraph = true;
            foreach (XmlNode child in node.ChildNodes)
                switch (child.Name) {
                    case "v":
                        verse = new Verse(chapter) {
                            Number = GetAttributeInt(child, "id"),
                            IsParagraphStart = isNewParagraph,
                        };
                        chapter.Verses.Add(verse);
                        isNewParagraph = false;
                        break;
                    case "w":
                        verse.Words.Add(ReadWord(verse, child));
                        break;
                    case "add":
                        verse.Words.Add(ReadWord(verse, child, true));
                        break;
                    case "#text":
                        if (verse != null) {
                            string text = GetSanitizedText(child);
                            bool isPunctuation = !char.IsLetterOrDigit(text.Trim()[0]);
                            if (isPunctuation) {
                                if (verse.Words.Count > 0)      // Punctuation at start of verse is ignored
                                    verse.Words.Last().Text += text;
                            } else
                                verse.Words.Add(ReadWord(verse, child));
                        }
                        break;
                    case "ve":
                        // Do nothing... Verse End marker
                        break;
                }
        }

        private string GetSanitizedText(XmlNode node) {
            return node.InnerText.Replace('\n', ' ').Trim();        // Verses with 'LORD' have this - e.g. Gen 2:4, also punctuation has new-lines (?)
        }
        private ElementWord ReadWord(Verse verse, XmlNode node, bool isAddedByTranslators = false) {
            ElementWord word = new ElementWord(verse) {
                Text = GetSanitizedText(node),
                IsAddedByTranslators = isAddedByTranslators,
            };

            string strongsNumbers = GetAttribute(node, "s", null);
            if (strongsNumbers != null)
                word.StrongsNumbers = strongsNumbers.Split(' ');

            return word;
        }
    }
}
