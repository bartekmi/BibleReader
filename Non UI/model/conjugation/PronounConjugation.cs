using BibleReader.model.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleReader.model.conjugation {
    public class PronounConjugation : ConjugationBase {
        public PronounType PronounType { get; set; }
        public Person Person { get; set; }
        public Gender Gender { get; set; }
        public Number Number { get; set; }

        public PronounConjugation() : base(PartOfSpeech.Pronoun) {
            // Do nothing
        }
    }
}
