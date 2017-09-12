using BibleReader.model.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleReader.model.conjugation {
    public class PrepositionConjugation : ConjugationBase {
        public PrepositionType PrepositionType { get; set; }

        public PrepositionConjugation() : base(PartOfSpeech.Preposition) {
            // Do nothing
        }
    }
}
