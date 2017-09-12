using BibleReader.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using BibleReader.model.enums;

namespace BibleReader.DataSource {
    public class LexiconSourceStrongs : LexiconSource {
        private const string FILENAME = @"Data\StrongHebrew.xml";

        public override Lexicon HydrateLexicon() {
            XmlDocument doc = new XmlDocument();
            doc.Load(FILENAME);
            XmlNode osis = doc.ChildNodes[1];
            XmlNode osisText = osis.ChildNodes[0];
            XmlNode glossary = osisText.ChildNodes[1];

            Lexicon lexicon = new Lexicon();

            foreach (XmlNode child in glossary.ChildNodes)
                if (child.Name == "div")
                    lexicon.AddEntry(ReadDefinition(child));

            return lexicon;
        }

        private LexiconWordDefinition ReadDefinition(XmlNode node) {
            LexiconWordDefinition definition = new LexiconWordDefinition();

            foreach (XmlNode child in node.ChildNodes)
                switch (child.Name) {
                    case "w":
                        definition.StrongsNumber = GetAttribute(child, "ID");
                        definition.OriginalLanguage = GetAttribute(child, "lemma");
                        definition.Pronounciation = GetAttribute(child, "POS");
                        ReadMorphology(definition, GetAttribute(child, "morph"));
                        break;
                    case "note":
                        string type = GetAttribute(child, "type");
                        if (type == "explanation") {
                            definition.Definitions = ReadDefinitions(child);
                            definition.Explanation = child;
                        } else if (type == "translation")
                            definition.Translation = child.InnerText.Trim('.');
                        else if (type == "exegesis") {
                            definition.Exegesis = child.InnerText;
                        }
                        break;
                    case "list":
                        definition.List = ReadList(child);
                        break;
                }

            return definition;
        }

        private string[] ReadList(XmlNode node) {
            List<string> strings = new List<string>();
            foreach (XmlNode child in node.ChildNodes) {
                string text = child.InnerText;
                int index = text.IndexOf(')');
                if (index > 0 && index < 5)
                    text = new string(' ', index * 3) + text;
                strings.Add(text);
            }

            return strings.ToArray();
        }

        private void ReadMorphology(LexiconWordDefinition definition, string morphology) {
            if (morphology.StartsWith("n-pr"))
                definition.PartOfSpeech = PartOfSpeech.Noun;        // Used have a "proper noun" enum, but now we have noun-type, so this would be redundant
            else if (morphology.StartsWith("n"))
                definition.PartOfSpeech = PartOfSpeech.Noun;
            else if (morphology.StartsWith("v"))
                definition.PartOfSpeech = PartOfSpeech.Verb;
            else if (morphology.StartsWith("adv"))
                definition.PartOfSpeech = PartOfSpeech.Adverb;
            else if (morphology.StartsWith("a"))
                definition.PartOfSpeech = PartOfSpeech.Adjective;
            else if (morphology.StartsWith("prep"))
                definition.PartOfSpeech = PartOfSpeech.Preposition;
            else if (morphology.StartsWith("inj"))
                definition.PartOfSpeech = PartOfSpeech.Interjection;

            if (morphology.EndsWith("-f"))
                definition.Gender = Gender.Feminine;
            else if (morphology.EndsWith("-m"))
                definition.Gender = Gender.Masculine;
        }

        private string[] ReadDefinitions(XmlNode node) {
            List<string> definitions = new List<string>();

            foreach (XmlNode child in node.ChildNodes)
                if (child.Name == "hi")
                    definitions.Add(child.InnerText);

            return definitions.ToArray();
        }
    }
}
