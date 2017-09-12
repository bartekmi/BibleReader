using BibleReader.model.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleReader.model.conjugation {
    public class ConjunctionConjugation : ConjugationBase {

        public ConjunctionConjugation() : base(PartOfSpeech.Conjunction) {
            // Do nothing
        }
    }
}
