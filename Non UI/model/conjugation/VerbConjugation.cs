using BibleReader.model.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleReader.model.conjugation {
    public class VerbConjugation : ConjugationBase {
        public VerbStem Stem { get; set; }
        public VerbForm Form { get; set; }
        public Person Person { get; set; }
        public Gender Gender { get; set; }
        public Number Number { get; set; }
        public State State { get; set; }

        public VerbConjugation() : base(PartOfSpeech.Verb) {
            // Do nothing
        }
    }
}
