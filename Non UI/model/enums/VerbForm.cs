using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleReader.model.enums {
    public enum VerbForm {
        Unknown,
        Perfect,
        Imperfect,
        PreteriteAndWawConsecutive,
        Jussive,
        Cohortative,
        Imperative,
        ActiveParticiple,
        PassiveParticiple,
        InfinitiveConstruct,
        InfinitiveAbsolute,

        // These were added based on Open Scriptures Morphology Project
        SequentialPerfect,
        SequentialImperfect,
    }
}
