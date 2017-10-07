using BibleReader.utils;
using BibleReader.model.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleReader.model {

    public enum HPunctuation {
        Maqaf = 0x5be,
        Paseq = 0x5c0,
        SofPasuq = 0x5c3,
        InvertedNun = 0x5c6,
    }

    public enum HLetter {
        Separator = 0x2f,

        // Pure letters with no embedded marks
        Aleph = 0x5d0,
        Bet = 0x5d1,
        Gimmel = 0x5d2,
        Dalet = 0x5d3,
        He = 0x5d4,
        Vav = 0x5d5,
        Zayin = 0x5d6,
        Het = 0x5d7,
        Tet = 0x5d8,
        Yod = 0x5d9,
        KafFinal = 0x5da,
        Kaf = 0x5db,
        Lamed = 0x5dc,
        MemFinal = 0x5dd,
        Mem = 0x5de,
        NunFinal = 0x5df,
        Nun = 0x5e0,
        Samekh = 0x5e1,
        Ayin = 0x5e2,
        PeFinal = 0x5e3,
        Pe = 0x5e4,
        TsadiFinal = 0x5e5,
        Tsadi = 0x5e6,
        Qof = 0x5e7,
        Resh = 0x5e8,
        Shin = 0x5e9,
        Tav = 0x5ea,
    }

    public enum LetterHasVowel {
        Yes,
        No,
        Sheva
    }

    public class Letter {

        #region Properties
        public HLetter TheLetter { get; set; }
        public HAnnotation[] Annotations { get; set; }

        // These cannot be determined from the context of the letter itself, but are set from the outside
        public bool IsMater { get; set; }
        public bool IsFurtivePatach { get; set; }

        // Derived
        public bool IsSeparator { get { return TheLetter == HLetter.Separator; } }

        public HAnnotation? Vowel {
            get {
                foreach (HAnnotation annotation in Annotations)
                    if (HebrewTextConversionUtils.IsVowelPoint(annotation))
                        return annotation;
                return null;
            }
        }

        public LetterHasVowel HasVowel {
            get {
                if (Vowel == null)
                    return LetterHasVowel.No;
                if (Vowel.Value == HAnnotation.PointSheva)
                    return LetterHasVowel.Sheva;
                return LetterHasVowel.Yes;
            }
        }

        // IBH 2.2
        public bool CouldBeMater {
            get {
                switch (TheLetter) {
                    case HLetter.Vav:
                        return HasDageshOrMapiq || Has(HAnnotation.PointHolam);
                    case HLetter.He:
                    case HLetter.Yod:
                        return HasVowel == LetterHasVowel.No;
                }

                return false;
            }
        }

        // http://www.hebrew4christians.com/Grammar/Unit_Three/Furtive_Patach/furtive_patach.html
        public bool CouldBeFurtivePatach {
            get {
                if (Has(HAnnotation.PointPatah))
                    switch (TheLetter) {
                        case HLetter.Het:
                        case HLetter.Ayin:
                            return true;
                        case HLetter.He:
                            if (HasDageshOrMapiq)
                                return true;
                            break;
                    }
                return false;
            }
        }

        public bool HasDageshOrMapiq { get { return Has(HAnnotation.PointDageshOrMapiq); } }
        public bool Has(HAnnotation annotation) {
            return Annotations.Contains(annotation);
        }

        // IBH-1.3.6
        public bool IsGuttural {
            get {
                switch (TheLetter) {
                    case HLetter.Aleph:
                    case HLetter.He:
                    case HLetter.Ayin:
                    case HLetter.Het:
                    case HLetter.Resh:
                        return true;
                }
                return false;
            }
        }

        // IBH-1.5
        public bool HasFinalForm {
            get {
                switch (TheLetter) {
                    case HLetter.Kaf:
                    case HLetter.Mem:
                    case HLetter.Nun:
                    case HLetter.Pe:
                    case HLetter.Tsadi:
                        return true;
                }
                return false;
            }
        }

        public HLetter TheLetterNonFinal {
            get {
                switch (TheLetter) {
                    case HLetter.KafFinal: return HLetter.Kaf;
                    case HLetter.MemFinal: return HLetter.Mem;
                    case HLetter.NunFinal: return HLetter.Nun;
                    case HLetter.PeFinal: return HLetter.Pe;
                    case HLetter.TsadiFinal: return HLetter.Tsadi;
                    default:
                        return TheLetter;
                }
            }
        }
        #endregion

        #region Constructors
        public Letter(HLetter letter, params HAnnotation[] annotations) {
            TheLetter = letter;
            Annotations = annotations;
        }

        // Copy constructor
        public Letter(Letter letter) {
            TheLetter = letter.TheLetter;
            Annotations = new List<HAnnotation>(letter.Annotations).ToArray();
        }
        #endregion

        #region Utilities

        public HAnnotation? GetVowel() {
            HAnnotation vowel = Annotations.SingleOrDefault(x => HAnnotationHelper.IsVowel(x));
            if ((int)vowel == 0)
                return null;
            return vowel;
        }

        public void ReplaceVowel(HAnnotation newVowel) {
            HAnnotation? originalVowel = GetVowel();
            if (originalVowel == null)
                Annotations = Annotations.Concat(new HAnnotation[] { newVowel }).ToArray();
            else {
                int index = Annotations.ToList().IndexOf(originalVowel.Value);
                Annotations[index] = newVowel;
            }
        }
        #endregion

        #region Equals
        public override bool Equals(object obj) {
            Letter other = (Letter)obj;
            return Equals(other, true);
        }

        public bool Equals(Letter other, bool isStrict) {
            if (TheLetter != other.TheLetter)
                return false;

            if (isStrict) 
                return AreAnnotationsEqualStrict(Annotations, other.Annotations);
            else
                return AreAnnotationsEqualLax(Annotations, other.Annotations);
        }

        private static bool AreAnnotationsEqualStrict(HAnnotation[] one, HAnnotation[] two) {
            HAnnotation[] these = one.OrderBy(x => x).ToArray();
            HAnnotation[] those = two.OrderBy(x => x).ToArray();

            if (these.Length != those.Length)
                return false;

            for (int ii = 0; ii < these.Length; ii++)
                if (these[ii] != those[ii])
                    return false;

            return true;
        }

        private static bool AreAnnotationsEqualLax(HAnnotation[] one, HAnnotation[] two) {
            // The problem with this is that silly things like accents cause differences. However, what about more
            // significant things like Shin-Dot and Dagesh? Can't think of a situation where this would make a difference
            // to conjugation, though.

            //HAnnotation[] oneNonVowels = one.Where(x => !HAnnotationHelper.IsVowel(x)).ToArray();
            //HAnnotation[] twoNonVowels = two.Where(x => !HAnnotationHelper.IsVowel(x)).ToArray();

            //if (!AreAnnotationsEqualStrict(oneNonVowels, twoNonVowels))
            //    return false;

            HAnnotation oneVowel = one.SingleOrDefault(x => HAnnotationHelper.IsVowel(x));
            HAnnotation twoVowel = two.SingleOrDefault(x => HAnnotationHelper.IsVowel(x));

            // An exact match on the vowel
            if (oneVowel == twoVowel)
                return true;

            // The two vowels belong to the same equivalent set
            foreach (HAnnotation[] set in INTERCHANGEABLE_VOWELS_SETS)
                if (set.Contains(oneVowel) && set.Contains(twoVowel))
                    return true;

            return false;
        }

        private static readonly HAnnotation[][] INTERCHANGEABLE_VOWELS_SETS = new HAnnotation[][] {
            new HAnnotation[] { HAnnotation.PointPatah, HAnnotation.PointQamats }
        };

        public override int GetHashCode() {
            List<int> list = new List<int>(Annotations.Select(x => (int)x).OrderBy(x => x));
            list.Add((int)TheLetter);

            int hash = list.Count;
            foreach (int ii in list)
                hash = unchecked(hash * 314159 + ii);

            return hash;
        }
        #endregion

        #region ToString()
        public override string ToString() {
            StringBuilder builder = new StringBuilder();

            builder.Append(TheLetter);
            if (Annotations.Length > 0) {
                builder.Append(": ");
                builder.Append(string.Join(", ", Annotations));
            }

            return builder.ToString();
        }
        #endregion
    }
}
