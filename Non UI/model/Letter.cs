using BibleReader.utils;
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

    public enum HAnnotation {
        // 0x59x
        AccentEtnahta = 0x591,
        AccentSegol = 0x592,
        AccentShalshelet = 0x593,
        AccentZaqefQatan = 0x594,
        AccentZeqefGadol = 0x595,
        AccentTipeha = 0x596,
        AccentRevia = 0x597,
        AccentZarqa = 0x598,
        AccentPashta = 0x599,
        AccentYetiv = 0x59a,
        AccentTevir = 0x59b,
        AccentGeresh = 0x59c,
        AccentGereshMuqdam = 0x59d,
        AccentGereshaim = 0x59e,
        AccentQarneyPara = 0x59f,

        // 0x5ax
        AccentTelishaGedola = 0x5a0,
        AccentPazer = 0x5a1,
        AccentAtnahHafukh = 0x5a2,
        AccentMunah = 0x5a3,
        AccentMahapakh = 0x5a4,
        AccentMerkha = 0x5a5,
        AccentMerkhaKefula = 0x5a6,
        AccentDarga = 0x5a7,
        AccentQadmah = 0x5a8,
        AccentTelishaQetana = 0x5a9,
        AccentYerahBenYomo = 0x5aa,
        AccentOle = 0x5ab,
        AccentIluy = 0x5ac,
        AccentDehi = 0x5ad,
        AccentZinor = 0x5ae,
        MarkMesoraCircle = 0x5af,

        // 0x5bx
        PointSheva = 0x5b0,
        PointHatafSegol = 0x5b1,
        PointHatafPatah = 0x5b2,
        PointHatafQamats = 0x5b3,
        PointHiriq = 0x5b4,
        PointTsere = 0x5b5,
        PointSegol = 0x5b6,
        PointPatah = 0x5b7,
        PointQamats = 0x5b8,
        PointHolam = 0x5b9,
        PointHolamHaserForVav = 0x5ba,
        PointQubuts = 0x5bb,
        PointDageshOrMapiq = 0x5bc,
        PointMeteg = 0x5bd,
        // Note that 0x5be is Maqaf, which is in the Punctuation enum
        PointRafe = 0x5bf,

        // 0x5cx
        // Note that 0x5c0 is Paseq, which is in the Punctuation enum
        PointShinDot = 0x5c1,
        PointSinDot = 0x5c2,

        MarkUpperDot = 0x5c4,
        MarkLowerDot = 0x5c5,
        PointQamatsQatan = 0x5c7,
    }

    public enum LetterHasVowel {
        Yes,
        No,
        Sheva
    }

    public class Letter {

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

        public Letter(HLetter letter, params HAnnotation[] annotations) {
            TheLetter = letter;
            Annotations = annotations;
        }

        public override bool Equals(object obj) {
            Letter other = (Letter)obj;
            if (TheLetter != other.TheLetter)
                return false;
            if (Annotations.Length != other.Annotations.Length)
                return false;

            HAnnotation[] these = Annotations.OrderBy(x => x).ToArray();
            HAnnotation[] those = Annotations.OrderBy(x => x).ToArray();

            for (int ii = 0; ii < these.Length; ii++)
                if (these[ii] != those[ii])
                    return false;

            return true;
        }

        public override int GetHashCode() {
            List<int> list = new List<int>(Annotations.Select(x => (int)x).OrderBy(x => x));
            list.Add((int)TheLetter);

            int hash = list.Count;
            foreach (int ii in list)
                hash = unchecked(hash * 314159 + ii);

            return hash;
        }

        public override string ToString() {
            StringBuilder builder = new StringBuilder();

            builder.Append(TheLetter);
            if (Annotations.Length > 0) {
                builder.Append(": ");
                builder.Append(string.Join(", ", Annotations));
            }

            return builder.ToString();
        }
    }
}
