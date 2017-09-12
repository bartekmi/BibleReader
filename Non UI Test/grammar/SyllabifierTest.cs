using BibleReader.model;
using BibleReader.utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleReader.grammar {
    [TestClass]
    public class SyllabifierTest {

        private Syllabifier _syllabifier = new Syllabifier();

        [TestMethod]
        public void TestSyllabify() {
            Test("בְּרֵאשִׁית", new Syllable("בְּ", SyllableType.Open), new Syllable("רֵא", SyllableType.Closed), new Syllable("שִׁית", SyllableType.Closed));
            Test("בָּרָ֣א", new Syllable("בָּ", SyllableType.Open), new Syllable("רָ֣א", SyllableType.Closed));
            Test("אֱלֹהִ֑ים", new Syllable("אֱ", SyllableType.Open), new Syllable("לֹ", SyllableType.Open), new Syllable("הִ֑ים", SyllableType.Closed));
            Test("הַ/שָּׁמַ֖יִם", new Syllable("הַ", SyllableType.Open), new Syllable("שָּׁ", SyllableType.Open), new Syllable("מַ֖", SyllableType.Open), new Syllable("יִם", SyllableType.Closed));
            Test("וְ/ר֣וּחַ", new Syllable("וְ", SyllableType.Open), new Syllable("ר֣וּחַ", SyllableType.Closed));
            Test("הָיְתָ֥ה", new Syllable("הָיְ", SyllableType.Closed), new Syllable("תָ֥ה", SyllableType.Open));
            Test("וְ/חֹ֖שֶׁךְ", new Syllable("וְ", SyllableType.Open), new Syllable("חֹ֖", SyllableType.Open), new Syllable("שֶׁךְ", SyllableType.Closed));
        }

        private void Test(string word, params Syllable[] expected) {
            Letter[] letters = HebrewTextConversionUtils.Extract(word);
            List<Syllable> actual = _syllabifier.Syllabify(letters);

            Assert.AreEqual(expected.Length, actual.Count, 
                string.Format("Expected: {0}. Actual: {1}",
                    string.Join(", ", expected.ToList()), string.Join(", ", actual)));

            for (int ii = 0; ii < expected.Length; ii++) {
                Syllable expectedSyl = expected[ii];
                Syllable actualSyl = actual[ii];
                string message = string.Format("Expected: {0}. Actual: {1}", expectedSyl, actualSyl);

                Assert.AreEqual(expectedSyl.Type, actualSyl.Type, message);
                Assert.AreEqual(expectedSyl.Letters.Length, actualSyl.Letters.Length, message);

                for (int jj = 0; jj < expectedSyl.Letters.Length; jj++) 
                    Assert.AreEqual(expectedSyl.Letters[jj].TheLetter, actualSyl.Letters[jj].TheLetter, message);
            }
        }
    }
}
