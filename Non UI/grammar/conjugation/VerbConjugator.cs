using BibleReader.DataSource;
using BibleReader.model;
using BibleReader.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BibleReader.model.enums;

namespace BibleReader.grammar.conjugation {
    public class VerbConjugator {
        // Conjugate a word which is known to be a verb.
        public VerbConjugationEntry Conjugate(ElementWord word) {
            if (word.Definitions.Length != 1)
                throw new Exception("Expecting exactly one definition for word " + word);

            Letter[] rootLetters = word.Definitions.Single().Letters;
            Letter[] wordLetters = word.Letters;
            List<VerbConjugationFamily> families = DetermineConjugationFamilies(rootLetters);

            foreach (VerbConjugationFamily family in families) {
                VerbConjugationEntry conjugation = Conjugate(wordLetters, rootLetters, family);
                if (conjugation != null)
                    return conjugation;
            }

            return null;
        }

        private VerbConjugationEntry Conjugate(Letter[] wordLetters, Letter[] rootLetters, VerbConjugationFamily family) {
            List<VerbConjugationEntry> conjugations = AppStaticsNonUI.Singleton.Conjugations.Get(family);
            VerbConjugationEntry qal3msg = conjugations.Single(x => x.Stem == VerbStem.Qal && x.Person == Person.Third && x.Gender == Gender.Masculine && x.Number == Number.Plural);
            Letter[] pattern = qal3msg.Letters;

            foreach (VerbConjugationEntry conjugation in conjugations) {
                Letter[] expansion = Expand(rootLetters, pattern, conjugation);
                if (HebrewTextConversionUtils.AreSame(expansion, wordLetters))
                    return conjugation;
            }

            return null;
        }

        private Letter[] Expand(Letter[] rootLetters /* 123 */, Letter[] pattern /* PQD */, VerbConjugationEntry conjugation /* Conjugated PQD */) {
            string[] patterns3 = new string[] {
                "321", 
            };
            throw new NotImplementedException();
        }

        private List<VerbConjugationFamily> DetermineConjugationFamilies(Letter[] root) {

            List<VerbConjugationFamily> families = new List<VerbConjugationFamily>();

            if (root.Length == 2)
                families.Add(VerbConjugationFamily.Hollow);

            if (root[0].IsGuttural)
                families.Add(VerbConjugationFamily.I_Guttural );
            if (root[0].TheLetter == HLetter.Aleph)
                families.Add(VerbConjugationFamily.I_Aleph);
            if (root[1].IsGuttural)
                families.Add(VerbConjugationFamily.II_Guttural);
            if (root[1].TheLetter == HLetter.Nun)
                families.Add(VerbConjugationFamily.I_Nun);
            if (root[0].TheLetter == HLetter.Yod) {
                families.Add(VerbConjugationFamily.I_Waw);
                families.Add(VerbConjugationFamily.I_Yod);
            }
            if (root.Length >= 3 && root[2].TheLetter == HLetter.He)
                families.Add(VerbConjugationFamily.III_He );
            if (root.Length >= 3 && root[1].TheLetter == root[2].TheLetter)
                families.Add(VerbConjugationFamily.Geminate );

            families.Add(VerbConjugationFamily.Regular);

            return families;
        }
    }
}
