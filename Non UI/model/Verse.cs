using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleReader.model
{
    public class Verse
    {
        public bool IsParagraphStart { get; set; }
        public int Number { get; set; }

        public Chapter Chapter { get; private set; }
        public List<Element> Words { get; private set; }

        private string _textOfVerseEsv;
        public string TextOfVerseEsv {
            get {
                if (_textOfVerseEsv == null) {
                    Chapter.PopulateOtherBibleVersions();
                }
                return _textOfVerseEsv;
            }
            internal set {
                _textOfVerseEsv = value;
            }
        }

        // Derived
        public string TextOfVerse { get { return string.Join(" ", Words.Select(x => x.Text).ToArray()); } }
        
        internal Verse(Chapter chapter)
        {
            Chapter = chapter;
            Words = new List<Element>();
        }

        public override string ToString()
        {
            return string.Format("{0} {1}:{2}", Chapter.Book.Abbreviation, Chapter.Number, Number);
        }
    }
}
