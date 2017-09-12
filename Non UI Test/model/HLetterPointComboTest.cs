using BibleReader.model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleReader.utils {
    [TestClass]
    public class HLetterPointComboTest {
        [TestMethod]
        public void TestNormalize() {
            List<HAnnotation> annotations = new List<HAnnotation>();

            HLetter letter = HLetterPointComboHelper.Normalize(HLetterPointCombo.ShinWithDageshAndSinDot, annotations);

            Assert.AreEqual(HLetter.Shin, letter);
            CollectionAssert.AreEquivalent(new HAnnotation[] { HAnnotation.PointSinDot, HAnnotation.PointDageshOrMapiq }, annotations);
        }
    }
}
