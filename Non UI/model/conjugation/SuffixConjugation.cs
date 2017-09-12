using BibleReader.model.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleReader.model.conjugation {
    public class SuffixConjugation : ConjugationBase {
        public SuffixType SuffixType { get; set; }
        public Person Person { get; set; }
        public Gender Gender { get; set; }
        public Number Number { get; set; }

        public SuffixConjugation() : base(PartOfSpeech.Suffix) {
            // Do nothing
        }
    }
}
