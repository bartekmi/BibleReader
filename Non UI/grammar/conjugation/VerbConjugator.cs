﻿using BibleReader.DataSource;
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
        public VerbConjugationEntry IdentifyConjugation(ElementWord verb) {
            if (verb.Definitions.Length != 1)
                throw new Exception("Expecting exactly one definition for word " + verb);

            LexiconWordDefinition definition = verb.Definitions.Single();
            if (definition.PartOfSpeech != PartOfSpeech.Verb)
                throw new Exception("Must be a verb");

            Letter[] rootLetters = definition.Letters;
            Letter[] wordLetters = verb.Letters;
            List<VerbConjugationFamily> families = DetermineConjugationFamilies(rootLetters);

            foreach (VerbConjugationFamily family in families) {
                VerbConjugationEntry conjugation = Conjugate(wordLetters, rootLetters, family);
                if (conjugation != null)
                    return conjugation;
            }

            return null;
        }

        private VerbConjugationEntry Conjugate(Letter[] wordLetters, Letter[] wordRoot, VerbConjugationFamily family) {
            List<VerbConjugationEntry> conjugations = AppStaticsNonUI.Singleton.Conjugations.Get(family);
            VerbConjugationEntry qal3msg = conjugations.Single(x => x.Stem == VerbStem.Qal && x.Person == Person.Third && x.Gender == Gender.Masculine && x.Number == Number.Singular);
            Letter[] paradigmRoot = qal3msg.Letters;

            foreach (VerbConjugationEntry conjugation in conjugations) {
                if (wordRoot.Length != paradigmRoot.Length)       // Can't possibly match because paradigm root and word root are different length
                    continue;

                Letter[] expansion = Expand(wordRoot, paradigmRoot, conjugation.Letters);
                if (HebrewTextConversionUtils.AreSame(expansion, wordLetters, false))
                    return conjugation;
            }

            return null;
        }

        private Letter[] Expand(Letter[] wordRoot /* בָּרָא */, Letter[] paradigmRoot /* פָקַד */, Letter[] paradigmConjugation /* פָּקַדְתָּ */) {

            List<Letter> expanded = new List<Letter>();

            int indexInRoot = 0;
            foreach (Letter letterInConjugation in paradigmConjugation) {
                Letter letterInParadigmRoot = indexInRoot < paradigmRoot.Length ? paradigmRoot[indexInRoot] : null;
                if (letterInParadigmRoot != null && letterInConjugation.TheLetter == letterInParadigmRoot.TheLetter) {
                    Letter expandedLetter = new Letter(wordRoot[indexInRoot]);

                    HAnnotation? vowel = letterInConjugation.GetVowel();
                    if (vowel != null)
                        expandedLetter.ReplaceVowel(vowel.Value);

                    expanded.Add(expandedLetter);
                    indexInRoot++;                       // Since we matched a paradigm letter, advance its index
                } else {
                    expanded.Add(letterInConjugation);      // prefix, infix, suffix
                }
            }

            return expanded.ToArray();
        }

        private List<VerbConjugationFamily> DetermineConjugationFamilies(Letter[] root) {

            List<VerbConjugationFamily> families = new List<VerbConjugationFamily>();

            if (root.Length == 2)
                families.Add(VerbConjugationFamily.Hollow);

            if (root[0].IsGuttural)
                families.Add(VerbConjugationFamily.I_Guttural);
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
                families.Add(VerbConjugationFamily.III_He);
            if (root.Length >= 3 && root[1].TheLetter == root[2].TheLetter)
                families.Add(VerbConjugationFamily.Geminate);

            families.Add(VerbConjugationFamily.Regular);

            return families;
        }
    }
}
