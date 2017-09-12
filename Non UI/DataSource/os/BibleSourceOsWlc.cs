using BibleReader.model;
using BibleReader.model.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using BibleReader.model.conjugation;

namespace BibleReader.DataSource.os {
    public class BibleSourceOsWlc : BibleSource {
        internal const string FOLDER = @"Data\OpenScriptures\Leningrad Codex";
        private static readonly object[] BOOKS_IN_ORDER = new object[] {
"Genesis", "Gen", BookEnum.Gen,
"Exodus", "Exod", BookEnum.Exo,
"Leviticus", "Lev", BookEnum.Lev,
"Numbers", "Num", BookEnum.Num,
"Deuteronomy", "Deut", BookEnum.Deu,
"Joshua", "Josh", BookEnum.Jos,
"Judges", "Judg", BookEnum.Jud,
"Ruth", "Ruth", BookEnum.Rut,
"1 Samuel", "1Sam", BookEnum._1Sa,
"2 Samuel", "2Sam", BookEnum._2Sa,
"1 Kings", "1Kgs", BookEnum._1Ki,
"2 Kings", "2Kgs", BookEnum._2Ki,
"1 Chronicles", "1Chr", BookEnum._1Ch,
"2 Chronicles", "2Chr", BookEnum._2Ch,
"Ezra", "Ezra", BookEnum.Ezr,
"Nehemiah", "Neh", BookEnum.Neh,
"Esther", "Esth", BookEnum.Est,
"Job", "Job", BookEnum.Job,
"Psalms", "Ps", BookEnum.Psa,
"Proverbs", "Prov", BookEnum.Pro,
"Ecclesiastes", "Eccl", BookEnum.Ecc,
"Song of Solomon", "Song", BookEnum.Sng,
"Isaiah", "Isa", BookEnum.Isa,
"Jeremiah", "Jer", BookEnum.Jer,
"Lamentations", "Lam", BookEnum.Lam,
"Ezekiel", "Ezek", BookEnum.Ezk,
"Daniel", "Dan", BookEnum.Dan,
"Hosea", "Hos", BookEnum.Hos,
"Joel", "Joel", BookEnum.Jol,
"Amos", "Amos", BookEnum.Amo,
"Obadiah", "Obad", BookEnum.Oba,
"Jonah", "Jonah", BookEnum.Jon,
"Micah", "Mic", BookEnum.Mic,
"Nahum", "Nah", BookEnum.Nam,
"Habakkuk", "Hab", BookEnum.Hab,
"Zephaniah", "Zeph", BookEnum.Zep,
"Haggai", "Hag", BookEnum.Hag,
"Zechariah", "Zech", BookEnum.Zec,
"Malachi", "Mal", BookEnum.Mal };

        public override Bible HydrateBible() {
            Bible bible = new Bible() {
                Name = "West Leningrad Codex - OS",
                Language = Language.Hebrew,
            };

            for (int ii = 0; ii < BOOKS_IN_ORDER.Length; ii += 3) {
                string fullName = BOOKS_IN_ORDER[ii].ToString();
                string shortName = BOOKS_IN_ORDER[ii + 1].ToString();
                BookEnum bookEnum = (BookEnum)BOOKS_IN_ORDER[ii + 2];
                string filename = string.Format(@"{0}\{1}.xml", FOLDER, shortName);

                Book book = new BookSourceOsWlc(bible, filename) {
                    Name = fullName,
                    Abbreviation = shortName,
                    BookEnum = bookEnum,
                };
                bible.Books.Add(book);
            }

            // Deal with Verse Map later...
            // 1) Read it
            // 2) Store each section with appropriate book
            // 3) When Hydrating the Book:
            //      a) Apply the Verse Map
            //      b) Store original Verse/Chapter for possible display

            return bible;
        }
    }

    internal class BookSourceOsWlc : Book {
        private bool _isHydrated = false;
        private string _filename;

        internal BookSourceOsWlc(Bible bible, string filename) : base(bible) {
            _filename = filename;
        }

        public override List<Chapter> Chapters {
            get {
                if (!_isHydrated) {
                    _isHydrated = true;
                    HydrateBook();
                }
                return base.Chapters;
            }
        }

        private void HydrateBook() {
            XmlDocument doc = new XmlDocument();
            doc.Load(_filename);
            XmlNode root = doc.ChildNodes[1];
            XmlNode content = root.FirstChild.LastChild;

            int number = 1;
            foreach (XmlNode chapterXml in content.ChildNodes) {
                Chapter chapter = new Chapter(this) {
                    Number = number++,
                };
                HydrateChapter(chapter, chapterXml);
                Chapters.Add(chapter);
            }
        }

        private void HydrateChapter(Chapter chapter, XmlNode chapterXml) {
            int number = 1;
            foreach (XmlNode verseXml in chapterXml.ChildNodes) {
                Verse verse = new Verse(chapter) {
                    Number = number++,
                };
                HydrateVerse(verse, verseXml);
                chapter.Verses.Add(verse);
            }
        }

        private void HydrateVerse(Verse verse, XmlNode verseXml) {
            foreach (XmlNode wordXml in verseXml.ChildNodes) {
                Element element = null;

                if (wordXml.Name == "w") {
                    element = new ElementWord(verse);
                    HydrateWord(element as ElementWord, wordXml);
                } else if (wordXml.Name == "seg") {
                    element = new ElementPunctuation(verse);
                    HydratePunctuation(element as ElementPunctuation, wordXml);
                } else if (wordXml.Name == "note") {
                    // Ignore notes at this time
                } else
                    throw new Exception("Unknown element name: " + wordXml.Name);

                if (element != null)
                    verse.Words.Add(element);
            }
        }

        private void HydrateWord(ElementWord word, XmlNode xml) {
            string strongsRaw = xml.Attributes["lemma"].Value;
            string strongs = CleanseStrongs(strongsRaw);

            word.StrongsNumbers = new string[] { strongs };
            word.RawStrongsNumberWithMarkings = strongsRaw;
            word.Text = xml.InnerText;

            XmlAttribute morphology = xml.Attributes["morph"];
            if (morphology != null)
                word.Conjugations = ExtractConjugation(morphology.Value);
        }

        private string CleanseStrongs(string strongsRaw) {
            StringBuilder builder = new StringBuilder();
            foreach (char c in strongsRaw)
                if (char.IsDigit(c))
                    builder.Append(c);

            return "H" + builder.ToString();
        }

        private void HydratePunctuation(ElementPunctuation punctuation, XmlNode xml) {
            punctuation.Text = xml.InnerText;
            string type = xml.Attributes["type"].Value;

            switch (type) {
                case "x-maqqef":
                    punctuation.Type = Punctuation.Maqqef;
                    break;
                case "x-sof-pasuq":
                    punctuation.Type = Punctuation.SofPasuq;
                    break;
                case "x-samekh":
                    punctuation.Type = Punctuation.Samekh;
                    break;
                case "x-pe":
                    punctuation.Type = Punctuation.Pe;
                    break;
                case "x-paseq":
                    punctuation.Type = Punctuation.Paseq;
                    break;
                case "x-reversednun":
                    punctuation.Type = Punctuation.ReversedNun;
                    break;
                default:
                    throw new Exception("Unknown punctuation type: " + type);
            }
        }

        #region Morphology
        private ConjugationBase[] ExtractConjugation(string morph) {
            morph = morph.Trim();
            if (morph.Length <= 1)
                return null;

            Language language;

            switch(morph[0]) {
                case 'H':
                    language = Language.Hebrew;
                    break;
                case 'A':
                    language = Language.Aramaic;
                    throw new NotImplementedException("Aramaic not implemented yet");
                default:
                    throw new Exception("Unknown Language: " + morph[0]);
            }

            morph = morph.Substring(1);
            return morph.Split('/').Select(x => ExtractSingleConjugation(x, language)).ToArray();
        }

        private ConjugationBase ExtractSingleConjugation(string morph, Language language) {
            char partOfSpeech = morph[0];
            morph = morph.Substring(1);

            switch (partOfSpeech) {
                case 'A':
                    return ExtractAdjectiveConjugation(morph);
                case 'C':
                    return new ConjunctionConjugation();
                case 'D':
                    return new AdverbConjugation();
                case 'N':
                    return ExtractNounConjugation(morph);
                case 'P':
                    return ExtractPronounConjugation(morph);
                case 'R':
                    return ExtractPrepositionConjugation(morph);
                case 'S':
                    return ExtractSuffixConjugation(morph);
                case 'T':
                    return ExtractParticleConjugation(morph);
                case 'V':
                    return ExtractVerbConjugation(morph, language);
                default:
                    throw new Exception("Unknown part of speech: " + partOfSpeech);
            }
        }

        private AdjectiveConjugation ExtractAdjectiveConjugation(string morph) {
            return new AdjectiveConjugation() {
                AdjectiveType = Extract(morph, 0, "acgo", AdjectiveType.Adjective, AdjectiveType.CardinalNumber, AdjectiveType.Gentilic, AdjectiveType.OrdinalNumber),
                Gender = ExtractGender(morph, 1),
                Number = ExtractNumber(morph, 2),
                State = ExtractState(morph, 3),
            };
        }

        private NounConjugation ExtractNounConjugation(string morph) {
            return new NounConjugation() {
                NounType = Extract(morph, 0, "cgp", NounType.Common, NounType.Gentilic, NounType.ProperName),
                Gender = ExtractGender(morph, 1),
                Number = ExtractNumber(morph, 2),
                State = ExtractState(morph, 3),
            };
        }

        private PronounConjugation ExtractPronounConjugation(string morph) {
            return new PronounConjugation() {
                PronounType = Extract(morph, 0, "dfipr", PronounType.Demonstrative, PronounType.Indefinite, PronounType.Interrogative, PronounType.Personal, PronounType.Relative),
                Person = ExtractPerson(morph, 1),
                Gender = ExtractGender(morph, 2),
                Number = ExtractNumber(morph, 3),
            };
        }

        private PrepositionConjugation ExtractPrepositionConjugation(string morph) {
            return new PrepositionConjugation() {
                PrepositionType = Extract(morph, 0, "d", PrepositionType.DefiniteArticle),
            };
        }

        private SuffixConjugation ExtractSuffixConjugation(string morph) {
            return new SuffixConjugation() {
                SuffixType = Extract(morph, 0, "dhnp", SuffixType.DirectionalHe, SuffixType.ParagogicHe, SuffixType.ParagogicNun, SuffixType.Pronomial),
                Person = ExtractPerson(morph, 1),
                Gender = ExtractGender(morph, 2),
                Number = ExtractNumber(morph, 3),
            };
        }

        private ParticleConjugation ExtractParticleConjugation(string morph) {
            return new ParticleConjugation() {
                ParticleType = Extract(morph, 0, "adeijmnor", ParticleType.Affirmation, ParticleType.DefiniteArticle, ParticleType.Exhortation, ParticleType.Interrogative, 
                    ParticleType.Interjection, ParticleType.Demonstrative, ParticleType.Negative, ParticleType.DirectObjectMarker, ParticleType.Relative),
            };
        }

        private VerbConjugation ExtractVerbConjugation(string morph, Language language) {
            return new VerbConjugation() {
                Stem = Extract(morph, 0, "qNpPhHtoOrmMkKQlLfDjiucvwyz", VerbStem.Qal, VerbStem.Niphal, VerbStem.Piel, VerbStem.Pual, VerbStem.Hiphil,
                                                                        VerbStem.Hophal, VerbStem.Hithpael, VerbStem.Polel, VerbStem.Polal, VerbStem.Hithpolel,
                                                                        VerbStem.Poel, VerbStem.Poal, VerbStem.Palel, VerbStem.Pulal, VerbStem.QalPassive,
                                                                        VerbStem.Pilpel, VerbStem.Polpal, VerbStem.Hithpalpel, VerbStem.Nithpael, VerbStem.Pealal,
                                                                        VerbStem.Pilel, VerbStem.Hothpaal, VerbStem.Tiphil, VerbStem.Hishtaphel, VerbStem.Nithpalel,
                                                                        VerbStem.Nithpoel, VerbStem.Hithpoel),

                Form = Extract(morph, 0, "pqiwhjvrsac", VerbForm.Perfect, VerbForm.SequentialPerfect, VerbForm.Imperfect, VerbForm.SequentialImperfect, VerbForm.Cohortative,
                                                        VerbForm.Jussive, VerbForm.Imperative, VerbForm.ActiveParticiple, VerbForm.PassiveParticiple, VerbForm.InfinitiveAbsolute,
                                                        VerbForm.InfinitiveConstruct),

                Person = ExtractPerson(morph, 2),
                Gender = ExtractGender(morph, 3),
                Number = ExtractNumber(morph, 4),
                State = ExtractState(morph, 5),
            };
        }

        #region helpers
        private Person ExtractPerson(string morph, int morphIndex) {
            return Extract(morph, morphIndex, "123", Person.First, Person.Second, Person.Third);
        }

        private Gender ExtractGender(string morph, int morphIndex) {
            return Extract(morph, morphIndex, "bcfm", Gender.BothNoun, Gender.CommonVerb, Gender.Feminine, Gender.Masculine);
        }

        private Number ExtractNumber(string morph, int morphIndex) {
            return Extract(morph, morphIndex, "dps", Number.Dual, Number.Plural, Number.Singular);
        }

        private State ExtractState(string morph, int morphIndex) {
            return Extract(morph, morphIndex, "acd", State.Absolute, State.Construct, State.Determined);
        }

        private T Extract<T>(string morph, int morphIndex, string codes, params T[] enums) {

            if (enums.Length != codes.Length)
                throw new Exception("Fix you code, please, 1.");
            if (codes.Distinct().Count() != codes.Length)
                throw new Exception("Fix you code, please, 2.");

            if (morphIndex >= morph.Length)
                return default(T);

            char c = morph[morphIndex];
            int codeIndex = codes.IndexOf(c);

            if (codeIndex == -1)
                return default(T);

            return enums[codeIndex];
        }
        #endregion
        #endregion
    }
}
