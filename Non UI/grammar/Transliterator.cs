using BibleReader.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleReader.grammar {
    // IBH-3.2
    public class Transliterator {

        public Syllabifier Syllabifier = new Syllabifier();
        public string SyllableSeparator = "-";

        public string Transliterate(Letter[] word) {
            List<Syllable> syllables = Syllabifier.Syllabify(word);

            StringBuilder builder = new StringBuilder();

            foreach (Syllable syllable in syllables) {
                if (builder.Length > 0)
                    builder.Append(SyllableSeparator);

                builder.Append(Transliterate(syllable));
            }

            string transliteration = builder.ToString();

            // A quiescent Aleph gets transliterated to an empty syllable, so we remove syllable sparator
            if (transliteration.StartsWith(SyllableSeparator))
                transliteration = transliteration.Substring(SyllableSeparator.Length);
            if (transliteration.EndsWith(SyllableSeparator))
                transliteration = transliteration.Substring(0, transliteration.Length - SyllableSeparator.Length);

            return transliteration;
        }

        private string Transliterate(Syllable syllable) {
            Letter[] letters = syllable.Letters;

            switch (letters.Length) {
                case 1:
                    if (syllable.Type == SyllableType.HolemVav)
                        return "u";
                    else
                        return
                            TransliterateConsonant(letters[0]) +
                            TransliterateVowel(letters[0]);
                case 2:
                    if (syllable.Type == SyllableType.Open)
                        return
                            TransliterateConsonant(letters[0]) +
                            TransliterateVowel(letters[0]) +
                            TransliterateMater(letters[1]);
                    else            // Closed Syllable
                        return
                            TransliterateConsonant(letters[0]) +
                            TransliterateVowel(letters[0]) +
                            TransliterateConsonant(letters[1]);
                case 3:
                    return
                        TransliterateConsonant(letters[0]) +
                        TransliterateVowel(letters[0]) +
                        TransliterateMater(letters[1]) +
                        TransliterateFurtivePatach(letters[2]) +
                        TransliterateConsonant(letters[2]);
                default:
                    throw new Exception("Whaaa??? A syllable with this many letters? " + syllable);
            }
        }

        private string TransliterateMater(Letter letter) {

            if (!letter.IsMater)
                throw new Exception("This letter is not a Mater: " + letter);

            switch (letter.TheLetterNonFinal) {
                case HLetter.Aleph: return "";
                case HLetter.He: return "h";
                case HLetter.Vav:
                    if (letter.Has(HAnnotation.PointDageshOrMapiq))
                        return "u";
                    else if (letter.Has(HAnnotation.PointHolam))
                        return "o";
                    else
                        throw new Exception("Vav is a Mater, but has neither Dagesh nor Holam");
                case HLetter.Yod: return "y";
                default:
                    throw new Exception("Unknown Mater: " + letter);
            }
        }

        private string TransliterateVowel(Letter letter) {
            HAnnotation? vowel = letter.Vowel;
            if (vowel == null)
                return "";

            switch (vowel.Value) {
                case HAnnotation.PointQamats:       // TODO: When is this to be pronouned 'o'. Are there rules for this?
                case HAnnotation.PointPatah:
                case HAnnotation.PointHatafPatah:
                    return "a";
                case HAnnotation.PointTsere:
                case HAnnotation.PointSegol:
                case HAnnotation.PointHatafSegol:
                case HAnnotation.PointSheva:
                    return "e";
                case HAnnotation.PointHiriq:
                    return "i";
                case HAnnotation.PointHolam:
                case HAnnotation.PointHolamHaserForVav:
                case HAnnotation.PointHatafQamats:
                    return "o";
                case HAnnotation.PointQubuts:
                    return "u";
                default:
                    throw new Exception("Unexpected vowel: " + vowel);
            }
        }

        private string TransliterateConsonant(Letter letter) {

            switch (letter.TheLetterNonFinal) {
                case HLetter.Aleph:
                    return letter.HasVowel == LetterHasVowel.No ? "" : "'";     // check for quiescent Aleph
                case HLetter.Bet:
                    return letter.HasDageshOrMapiq ? "b" : "v";
                case HLetter.Gimmel: return "g";
                case HLetter.Dalet: return "d";
                case HLetter.He: return "h";
                case HLetter.Vav: return "v";
                case HLetter.Zayin: return "z";
                case HLetter.Het: return "ch";
                case HLetter.Tet: return "t";
                case HLetter.Yod:
                    return letter.IsMater ? "" : "y";
                case HLetter.Kaf:
                    return letter.HasDageshOrMapiq ? "k" : "ch";
                case HLetter.Lamed: return "l";
                case HLetter.Mem: return "m";
                case HLetter.Nun: return "n";
                case HLetter.Samekh: return "s";
                case HLetter.Ayin: return "'";
                case HLetter.Pe:
                    return letter.HasDageshOrMapiq ? "p" : "f";
                case HLetter.Tsadi: return "tz";
                case HLetter.Qof: return "q";
                case HLetter.Resh: return "r";
                case HLetter.Shin:
                    if (letter.Has(HAnnotation.PointShinDot))
                        return "sh";
                    else if (letter.Has(HAnnotation.PointSinDot))
                        return "s";
                    else
                        return "sh?";
                case HLetter.Tav: return "t";
                default:
                    throw new Exception("Unexpected consonant letter: " + letter);
            }
        }

        private string TransliterateFurtivePatach(Letter letter) {
            return letter.CouldBeFurtivePatach ? "a" : "";
        }
    }
}
