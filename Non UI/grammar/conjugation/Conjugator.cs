using BibleReader.model;
using BibleReader.model.conjugation;
using BibleReader.model.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleReader.grammar.conjugation {
    public static class Conjugator {
        public static ConjugationBase[] Conjugate(ElementWord word) {
            List<ConjugationBase> conjugations = new List<ConjugationBase>();

            foreach (LexiconWordDefinition definition in word.Definitions) {
                switch (definition.PartOfSpeech) {
                    case PartOfSpeech.Verb:
                        conjugations.AddRange(new VerbConjugator().IdentifyConjugation(word));
                        break;
                    // TODO: More conjugators here
                }
            }

            return conjugations.ToArray();
        }
    }
}
