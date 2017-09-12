using BibleReader.model.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleReader.model.conjugation {
    public class ParticleConjugation : ConjugationBase {
        public ParticleType ParticleType { get; set; }

        public ParticleConjugation() : base(PartOfSpeech.Particle) {
            // Do nothing
        }
    }
}
