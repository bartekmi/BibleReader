using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleReader.model {
    public class Lexicon {
        private Dictionary<string, LexiconWordDefinition> _dictionary { get; set; }

        public Lexicon() {
            _dictionary = new Dictionary<string, LexiconWordDefinition>();
        }

        internal void AddEntry(LexiconWordDefinition definition) {
            _dictionary[definition.StrongsNumber] = definition;
        }

        public LexiconWordDefinition LookUp(string strongsNumber) {
            LexiconWordDefinition definition = null;
            _dictionary.TryGetValue(strongsNumber, out definition);
            return definition;
        }

        public void GenerateIndex(Bible bible) {
            foreach (Book book in bible.Books)
                foreach (Chapter chapter in book.Chapters)
                    foreach (Verse verse in chapter.Verses)
                        foreach (ElementWord word in verse.Words.OfType<ElementWord>().Where(x => x.StrongsNumbers != null))
                            foreach (string strongsNumber in word.StrongsNumbers) {
                                LexiconWordDefinition definition;
                                if (_dictionary.TryGetValue(strongsNumber, out definition))
                                    definition.References.Add(word);
                            }
        }

        public ElementWord FirstOccurrence(string strongsNumber) {
            LexiconWordDefinition definition;
            if (_dictionary.TryGetValue(strongsNumber, out definition))
                return definition.References.FirstOrDefault();
            return null;
        }
    }
}
