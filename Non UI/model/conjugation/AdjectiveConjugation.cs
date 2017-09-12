using BibleReader.model.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleReader.model.conjugation {
    public class AdjectiveConjugation : ConjugationBase {
        public AdjectiveType AdjectiveType { get; set; }
        public Gender Gender { get; set; }
        public Number Number { get; set; }
        public State State { get; set; }

        public AdjectiveConjugation() : base(PartOfSpeech.Adjective) {
            // Do nothing
        }
    }
}
