using BibleReader.model.conjugation;
using BibleReader.model.enums;
using BibleReader.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleReader.model {
    public class ElementWord : Element {
        public bool IsAddedByTranslators { get; set; }
        public string[] StrongsNumbers { get; set; }
        public string RawStrongsNumberWithMarkings { get; set; }

        public ConjugationBase[] Conjugations { get; set; }

        // Derived Properties
        public LexiconWordDefinition[] Definitions {
            get {
                if (StrongsNumbers != null)
                    return StrongsNumbers.Select(x => AppStaticsNonUI.Singleton.CurrentLexison.LookUp(x)).ToArray();
                return null;
            }
        }

        public string OriginalLanguage {
            get {
                LexiconWordDefinition[] definitions = Definitions;
                if (definitions == null)
                    return null;
                return string.Join(" ", definitions.Select(x => x == null ? "" : x.OriginalLanguage));
            }
        }

        private Letter[] _letters;
        public Letter[] Letters {
            get {
                if (_letters == null) 
                    _letters = HebrewTextConversionUtils.Extract(Text);
                return _letters;
            }
        }

        public ElementWord(Verse verse) : base(verse) {
            // Do nothing
        }

        private const int NUM_BEFORE_AFTER = 5;
        public string TextBefore {
            get {
                int index = Verse.Words.IndexOf(this);
                return IsFirstInVerse ? "" : PartialVerse(Verse, index - NUM_BEFORE_AFTER - 1, index - 1);
            }
        }

        public string TextAfter {
            get {
                int index = Verse.Words.IndexOf(this);
                return IsLastInVerse ? "" : PartialVerse(Verse, index + 1, index + NUM_BEFORE_AFTER);
            }
        }

        private string PartialVerse(Verse verse, int from, int to) {
            from = Limit(verse, from);
            to = Limit(verse, to);

            return string.Join(" ", verse.Words.Skip(from).Take(to - from + 1).Select(x => x.Text));
        }

        private int Limit(Verse verse, int index) {
            if (index < 0)
                index = 0;

            if (index >= verse.Words.Count)
                index = verse.Words.Count - 1;

            return index;
        }
    }
}
