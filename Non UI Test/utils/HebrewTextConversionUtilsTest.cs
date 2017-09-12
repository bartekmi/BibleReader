using BibleReader.model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleReader.utils {
    [TestClass]
    public class HebrewTextConversionUtilsTest {
        [TestMethod]
        public void TestExtractHewbrew() {
            VerifyLetters(new Letter[] {
                new Letter(HLetter.Bet, HAnnotation.PointQamats, HAnnotation.PointDageshOrMapiq),
                new Letter(HLetter.Resh, HAnnotation.PointQamats, HAnnotation.AccentMunah),
                new Letter(HLetter.Aleph),
            }, "בָּרָ֣א");      // Bara - Created

            VerifyLetters(new Letter[] {
                new Letter(HLetter.Aleph, HAnnotation.PointHatafSegol),
                new Letter(HLetter.Lamed, HAnnotation.PointHolam),
                new Letter(HLetter.He, HAnnotation.PointHiriq, HAnnotation.AccentEtnahta),
                new Letter(HLetter.Yod),
                new Letter(HLetter.MemFinal),
            }, "אֱלֹהִ֑ים");      // Elohim - God


            VerifyLetters(new Letter[] {
                new Letter(HLetter.He, HAnnotation.PointPatah),
                new Letter(HLetter.Separator),
                new Letter(HLetter.Shin, HAnnotation.PointShinDot, HAnnotation.PointQamats, HAnnotation.PointDageshOrMapiq),
                new Letter(HLetter.Mem, HAnnotation.PointPatah, HAnnotation.AccentTipeha),
                new Letter(HLetter.Yod, HAnnotation.PointHiriq),
                new Letter(HLetter.MemFinal),
            }, "הַ/שָּׁמַ֖יִם");      // Ha-Shamaim - The Heavens

            VerifyLetters(new Letter[] {
                new Letter(HLetter.Vav, HAnnotation.PointSheva),
                new Letter(HLetter.Separator),
                new Letter(HLetter.Resh, HAnnotation.AccentMunah),
                new Letter(HLetter.Vav, HAnnotation.PointDageshOrMapiq),
                new Letter(HLetter.Het, HAnnotation.PointPatah),    // Note that furtive patach is not distinguished
            }, "וְ/ר֣וּחַ");      // Ve-Ruach - And the Spirit...

            VerifyLetters(new Letter[] {
                new Letter(HLetter.Tav, HAnnotation.PointHolam),
                new Letter(HLetter.He),
                new Letter(HLetter.Vav, HAnnotation.PointDageshOrMapiq)
            }, "תֹהוּ");      // Tohu - Formless

        }

        private void VerifyLetters(Letter[] expected, string word) {
            Letter[] actual = HebrewTextConversionUtils.Extract(word);

            Assert.AreEqual(expected.Length, actual.Length);

            for (int ii = 0; ii < expected.Length; ii++)
                Assert.AreEqual(expected[ii], actual[ii]);
        }
    }
}
