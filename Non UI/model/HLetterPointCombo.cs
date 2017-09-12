using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BibleReader.model {
    public enum HLetterPointCombo {
        // 0xfb1x
        [Lpc(HLetter.Yod, HAnnotation.PointHiriq)]
        YodWithHiriq = 0xfb1d,

        // 0xfb2x
        [Lpc(HLetter.Shin, HAnnotation.PointShinDot)]
        ShinWithShinDot = 0xfb2a,
        [Lpc(HLetter.Shin, HAnnotation.PointSinDot)]
        ShinWithSinDot = 0xfb2b,
        [Lpc(HLetter.Shin, HAnnotation.PointDageshOrMapiq, HAnnotation.PointShinDot)]
        ShinWithDageshAndShinDot = 0xfb2c,
        [Lpc(HLetter.Shin, HAnnotation.PointDageshOrMapiq, HAnnotation.PointSinDot)]
        ShinWithDageshAndSinDot = 0xfb2d,
        [Lpc(HLetter.Aleph, HAnnotation.PointPatah)]
        AlefWithPatah = 0xfb2e,
        [Lpc(HLetter.Aleph, HAnnotation.PointQamats)]
        AlefWithQamats = 0xfb2f,

        // 0xfb3x
        [Lpc(HLetter.Aleph, HAnnotation.PointDageshOrMapiq)]
        AlefWithMapiq = 0xfb30,
        [Lpc(HLetter.Bet, HAnnotation.PointDageshOrMapiq)]
        BetWithDagesh = 0xfb31,
        [Lpc(HLetter.Gimmel, HAnnotation.PointDageshOrMapiq)]
        GimelWithDagesh = 0xfb32,
        [Lpc(HLetter.Dalet, HAnnotation.PointDageshOrMapiq)]
        DaletWithDagesh = 0xfb33,
        [Lpc(HLetter.He, HAnnotation.PointDageshOrMapiq)]
        HeWithMapiq = 0xfb34,
        [Lpc(HLetter.Vav, HAnnotation.PointDageshOrMapiq)]
        VavWithDagesh = 0xfb35,
        [Lpc(HLetter.Zayin, HAnnotation.PointDageshOrMapiq)]
        ZayinWithDagesh = 0xfb36,
        [Lpc(HLetter.Tet, HAnnotation.PointDageshOrMapiq)]
        TetWithDagesh = 0xfb38,
        [Lpc(HLetter.Yod, HAnnotation.PointDageshOrMapiq)]
        YodWithDagesh = 0xfb39,
        [Lpc(HLetter.KafFinal, HAnnotation.PointDageshOrMapiq)]
        FinalKafWithDagesh = 0xfb3a,
        [Lpc(HLetter.Kaf, HAnnotation.PointDageshOrMapiq)]
        KafWithDagesh = 0xfb3b,
        [Lpc(HLetter.Lamed, HAnnotation.PointDageshOrMapiq)]
        LamedWithDagesh = 0xfb3c,
        [Lpc(HLetter.Mem, HAnnotation.PointDageshOrMapiq)]
        MemWithDagesh = 0xfb3e,

        // 0xfb4x
        [Lpc(HLetter.Nun, HAnnotation.PointDageshOrMapiq)]
        NunWithDagesh = 0xfb40,
        [Lpc(HLetter.Samekh, HAnnotation.PointDageshOrMapiq)]
        SamekhWithDagesh = 0xfb41,
        [Lpc(HLetter.PeFinal, HAnnotation.PointDageshOrMapiq)]
        FinalPeWithDagesh = 0xfb43,
        [Lpc(HLetter.Pe, HAnnotation.PointDageshOrMapiq)]
        PeWithDagesh = 0xfb44,
        [Lpc(HLetter.Tsadi, HAnnotation.PointDageshOrMapiq)]
        TsadiWithDagesh = 0xfb46,
        [Lpc(HLetter.Qof, HAnnotation.PointDageshOrMapiq)]
        QofWithDagesh = 0xfb47,
        [Lpc(HLetter.Resh, HAnnotation.PointDageshOrMapiq)]
        ReshWithDagesh = 0xfb48,
        [Lpc(HLetter.Shin, HAnnotation.PointDageshOrMapiq, HAnnotation.PointShinDot)]
        ShinWithDagesh = 0xfb49,
        [Lpc(HLetter.Tav, HAnnotation.PointDageshOrMapiq)]
        TavWithDagesh = 0xfb4a,
        [Lpc(HLetter.Vav, HAnnotation.PointHolam)]
        VavWithHolam = 0xfb4b,

        // Not sure what to do with these ones, yet
        BetWithRafe = 0xfb4c,
        KafWithRafe = 0xfb4d,
        PeWithRafe = 0xfb4e,
    }

    internal static class HLetterPointComboHelper {
        internal static HLetter Normalize(HLetterPointCombo c, ICollection<HAnnotation> annotations) {
            MemberInfo info = typeof(HLetterPointCombo).GetMember(c.ToString()).Single();
            LpcAttribute attribute = (LpcAttribute)info.GetCustomAttributes(typeof(LpcAttribute), false).Single();
            if (attribute == null)
                throw new Exception("Don't yet know what to do with HLetterPointCombo: " + c);

            foreach (HAnnotation annotation in attribute.Annotations)
                annotations.Add(annotation);

            return attribute.Letter;
        }
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class LpcAttribute : Attribute {
        public readonly HLetter Letter;
        public readonly HAnnotation[] Annotations;

        public LpcAttribute(HLetter letter, params HAnnotation[] annotations) {
            Letter = letter;
            Annotations = annotations;
        }
    }

}
