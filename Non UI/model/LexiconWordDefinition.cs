using BibleReader.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using BibleReader.model.enums;

namespace BibleReader.model
{
    public class LexiconWordDefinition
    {
        public string StrongsNumber { get; set; }
        public string OriginalLanguage { get; set; }
        public string[] Definitions { get; set; }
        public PartOfSpeech PartOfSpeech { get; set; }
        public Gender? Gender { get; set; }
        public XmlNode Explanation { get; set; }
        public string Translation { get; set; }
        public string Pronounciation { get; set; }
        public string Transliteration { get; set; }
        public string[] List { get; set; }

        public LexiconWordDefinition[] ExegesisRoots { get; set; }
        public string Exegesis { get; set; }

        // Derived and Post-processed
        public string Title { get { return Definitions.FirstOrDefault(); } }
        public List<ElementWord> References { get; private set; }
        public int Frequency { get { return References.Count; } }
        public string ListAsString { get { return string.Join("\r\n", List); } }


        private Letter[] _letters;
        public Letter[] Letters {
            get {
                if (_letters == null)
                    _letters = HebrewTextConversionUtils.Extract(OriginalLanguage);
                return _letters;
            }
        }

        public LexiconWordDefinition() {
            References = new List<ElementWord>();
        }

        public ElementWord FirstReference() {
            return References.FirstOrDefault();
        }

        public override string ToString() {
            return Title;
        }
    }
}
