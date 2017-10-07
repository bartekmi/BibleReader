using BibleReader.DataSource;
using BibleReader.model;
using BibleReader.model.conjugation;
using BibleReader.model.enums;
using BibleReader.utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleReader.grammar.conjugation {
    [TestClass]
    public class VerbConjugatorTest {

        private VerbConjugator _conjugator = new VerbConjugator();

        [TestInitialize]
        public void TestInitialize() {
            new AppStaticsNonUI() {
                CurrentLexison = new Lexicon(),
            };
        }

        [TestMethod]
        public void TestConjugateRegular() {

            AppStaticsNonUI.Singleton.CurrentLexison.AddEntry(new LexiconWordDefinition() {
                StrongsNumber = "H1254",
                OriginalLanguage = "בּרא",
                PartOfSpeech = PartOfSpeech.Verb,
            });

            ElementWord verb = new ElementWord(null) {
                Text = "בָּרָ֣א",
                StrongsNumbers = new string[] {"H1254"},
            };

            VerbConjugation conjugation = _conjugator.IdentifyConjugation(verb).Single();

            Assert.IsNotNull(conjugation);

            Assert.AreEqual(VerbConjugationFamily.II_Guttural, conjugation.Family);
            Assert.AreEqual(VerbForm.Perfect, conjugation.Form);
            Assert.AreEqual(Gender.Masculine, conjugation.Gender);
            Assert.AreEqual(Number.Singular, conjugation.Number);
            Assert.AreEqual(Person.Third, conjugation.Person);
            Assert.AreEqual(VerbStem.Qal, conjugation.Stem);
        }
    }
}
