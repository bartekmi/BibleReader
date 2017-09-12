using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BibleReader.model;

namespace BibleReader.apis {
    [TestClass]
    public class EsvApiTest {

        private EsvApi _api = new EsvApi();

        [TestMethod]
        public void TestGetChapter() {
            Bible bible = new Bible();
            Book book = new Book(bible) {
                Name = "Genesis"
            };
            Chapter chapter = new Chapter(book) {
                Number = 1
            };
            string[] verses = _api.GetChapter(chapter);

            Assert.AreEqual(31, verses.Length);
            Assert.AreEqual("In the beginning, God created the heavens and the earth. (ESV)", verses[0]);

        }

        [TestMethod]
        public void TestGetChapterWithEmbeddedBrackets() {
            Bible bible = new Bible();
            Book book = new Book(bible) {
                Name = "Psalms"
            };
            Chapter chapter = new Chapter(book) {
                Number = 145
            };
            string[] verses = _api.GetChapter(chapter);

            Assert.AreEqual(21, verses.Length);
            AssertUtils.AssertStringsEqual(
@"Your kingdom is an everlasting kingdom,
    and your dominion endures throughout all generations.

  [The LORD is faithful in all his words
    and kind in all his works.] (ESV)", verses[12].Replace("\n", "\r\n"));
        }
    }
}
