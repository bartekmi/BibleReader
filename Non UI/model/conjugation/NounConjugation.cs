using BibleReader.model.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleReader.model.conjugation {
    public class NounConjugation : ConjugationBase {
        public NounType NounType { get; set; }
        public Gender Gender { get; set; }
        public Number Number { get; set; }
        public State State { get; set; }

        public NounConjugation() : base(PartOfSpeech.Noun) {
            // Do nothing
        }
    }
}
