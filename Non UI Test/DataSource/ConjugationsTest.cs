using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BibleReader.model.enums;

namespace BibleReader.DataSource {
    [TestClass]
    public class ConjugationsTest {

        private Conjugations _conjugations;

        [TestInitialize]
        public void Initialize() {
            _conjugations = new Conjugations();
        }

        [TestMethod]
        public void TestConjugationsConstructor() {

            Verify("פָקַד", new VerbConjugationEntry() {
                Family = VerbConjugationFamily.Regular,
                Stem = VerbStem.Qal,
                Form = VerbForm.Perfect,
                Person = Person.Third,
                Gender = Gender.Masculine,
                Number = Number.Singular,
            });

            Verify("פְּקַדְתֶּם", new VerbConjugationEntry() {
                Family = VerbConjugationFamily.Regular,
                Stem = VerbStem.Qal,
                Form = VerbForm.Perfect,
                Person = Person.Second,
                Gender = Gender.Masculine,
                Number = Number.Plural,
            });
        }

        private void Verify(string text, VerbConjugationEntry expected) {
            VerbConjugationEntry actual = _conjugations.Get(expected.Family).Single(x => x.Text == text);

            Assert.AreEqual(expected.Family, actual.Family);
            Assert.AreEqual(expected.Stem, actual.Stem);
            Assert.AreEqual(expected.Form, actual.Form);
            Assert.AreEqual(expected.Person, actual.Person);
            Assert.AreEqual(expected.Gender, actual.Gender);
            Assert.AreEqual(expected.Number, actual.Number);
        }
    }
}
