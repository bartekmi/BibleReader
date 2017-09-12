using BibleReader.DataSource.os;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleReader.model.conjugation {
    [TestClass]
    public class ConjugationBaseTest {

        private BookSourceOsWlc _genesis;

        [TestInitialize]
        public void TestInitialize() {
            _genesis = new BookSourceOsWlc(new Bible(), @"Data\OpenScriptures\Leningrad Codex\Gen.xml");
        }

        [TestMethod]
        public void TestConjugationBaseToString() {
            Chapter chapter1 = _genesis.Chapters[0];
            Verse verse1 = chapter1.Verses[0];

            // Bereshit
            ElementWord word1 = (ElementWord)verse1.Words[0];
            Assert.AreEqual(2, word1.Conjugations.Length);

            AssertUtils.AssertStringsEqual(
@"Part of Speech: Preposition
PrepositionType: Unknown", 
    word1.Conjugations[0].ToString());

            AssertUtils.AssertStringsEqual(
@"Part of Speech: Noun
NounType: Common
Gender: Feminine
Number: Singular
State: Absolute", 
    word1.Conjugations[1].ToString());

            // Bara
            ElementWord word2 = (ElementWord)verse1.Words[1];
            Assert.AreEqual(1, word2.Conjugations.Length);

            AssertUtils.AssertStringsEqual(
@"Part of Speech: Verb
Stem: Qal
Form: SequentialPerfect
Person: Third
Gender: Masculine
Number: Singular
State: Unknown",
    word2.Conjugations[0].ToString());
        }
    }
}
