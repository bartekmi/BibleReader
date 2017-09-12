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
    public class TransliteratorTest {

        private Transliterator _transliterator = new Transliterator();

        [TestMethod]
        public void TestTransliterate() {
            Test("וַ/יַּ֧רְא", "va-yar");
            Test("בְּרֵאשִׁית", "be-re-shiyt");
            Test("בָּרָ֣א", "ba-ra");
            Test("אֱלֹהִ֑ים", "'e-lo-hiym");
            Test("הַ/שָּׁמַ֖יִם", "ha-sha-ma-yim");
            Test("הָאָרֶץ", "ha-'a-retz");
            Test("הָיְתָ֥ה", "hay-tah");
            Test("תֹהוּ", "to-hu");
            Test("וְ/ר֣וּחַ", "ve-ruach");
            Test("וְ/חֹ֖שֶׁךְ", "ve-cho-shech");
            Test("א֑וֹר", "or");
            Test("וּ/בֵ֥ין", "u-veyn");
        }

        private void Test(string word, string expectedTransliteration) {
            Letter[] letters = HebrewTextConversionUtils.Extract(word);
            string actualTransliteration = _transliterator.Transliterate(letters);
            Assert.AreEqual(expectedTransliteration, actualTransliteration);
        }
    }
}
