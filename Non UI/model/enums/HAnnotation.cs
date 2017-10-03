using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleReader.model.enums {
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

    public static class HAnnotationHelper {
        public static bool IsVowel(HAnnotation annotation) {
            return
                annotation >= HAnnotation.PointSheva && 
                annotation <= HAnnotation.PointQubuts;
        }
    }
}
