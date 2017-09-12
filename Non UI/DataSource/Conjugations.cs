using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx;
using BibleReader.utils;
using Telerik.Windows.Documents.Spreadsheet.Formatting.FormatStrings;
using BibleReader.model;
using BibleReader.model.enums;

namespace BibleReader.DataSource {

    public class VerbConjugationEntry {
        public VerbConjugationFamily Family;
        public VerbStem Stem;
        public VerbForm Form;
        public Person Person;
        public Gender Gender;
        public Number Number;
        public string Text;
        public Letter[] Letters;
    }

    public class Conjugations {

        private Dictionary<VerbConjugationFamily, List<VerbConjugationEntry>> _data = new Dictionary<VerbConjugationFamily, List<VerbConjugationEntry>>();
        private Cells _cells;
        private CellIndex _range;

        public Conjugations() {
            foreach (string name in new string[] { "Regular" }) {
                string resourceName = string.Format("BibleReader.data.Conjugation.{0}.xlsx", name);
                XlsxFormatProvider formatter = new XlsxFormatProvider();
                Workbook workbook = formatter.Import(ResourceUtils.ReadEmbeddedResource(resourceName));
                VerbConjugationFamily family = EnumUtils.Parse<VerbConjugationFamily>(name);
                Sheet sheet = workbook.Sheets.First();
                _data[family] = ReadSheet(family, (Worksheet)sheet);
            }
        }

        public List<VerbConjugationEntry> Get(VerbConjugationFamily family) {
            return _data[family];
        }

        private List<VerbConjugationEntry> ReadSheet(VerbConjugationFamily family, Worksheet sheet) {

            List<VerbConjugationEntry> data = new List<VerbConjugationEntry>();

            _cells = sheet.Cells;
            _range = sheet.UsedCellRange.ToIndex;

            int endRow = _range.RowIndex;
            int columnCount = _range.ColumnIndex;
            VerbForm? form = null;

            List<VerbStem> stems = ReadStems();

            for (int iRow = 1; iRow <= endRow; iRow++) {

                string formString = GetValue(iRow, 0);
                if (!string.IsNullOrWhiteSpace(formString))
                    form = EnumUtils.Parse<VerbForm>(formString);

                string grammarString = GetValue(iRow, 1);
                Person person; Gender gender; Number number;
                ParseGrammar(grammarString, out person, out gender, out number);

                for (int iCol = 2; iCol <= columnCount; iCol++) {
                    string text = GetValue(iRow, iCol);
                    if (string.IsNullOrWhiteSpace(text))
                        continue;

                    data.Add(new VerbConjugationEntry() {
                        Family = family,
                        Stem = stems[iCol - 2],
                        Form = form.Value,
                        Person = person,
                        Gender = gender,
                        Number = number,
                        Text = text,
                        Letters = HebrewTextConversionUtils.Extract(text),
                    });
                }
            }

            return data;
        }

        private List<VerbStem> ReadStems() {
            int columnCount = _range.ColumnIndex;
            List<VerbStem> stems = new List<VerbStem>();

            for (int iCol = 2; iCol <= columnCount; iCol++)
                stems.Add(EnumUtils.Parse<VerbStem>(GetValue(0, iCol)));

            return stems;
        }

        private void ParseGrammar(string str, out Person person, out Gender gender, out Number number) {

            if (string.IsNullOrWhiteSpace(str)) {
                person = Person.NotApplicable;
                gender = Gender.Unknown;
                number = Number.Unknown;
                return;
            }

            if (str.Length == 3)
                str = "0" + str;

            string personStr = str.Substring(0, 1);
            switch (personStr) {
                case "0": person = Person.NotApplicable; break;
                case "1": person = Person.First; break;
                case "2": person = Person.Second; break;
                case "3": person = Person.Third; break;
                default:
                    throw new Exception(string.Format("Unexpected person '{0}' in '{1}'", personStr, str));
            }

            string genderStr = str.Substring(1, 1);
            switch (genderStr) {
                case "m": gender = Gender.Masculine; break;
                case "f": gender = Gender.Feminine; break;
                case "c": gender = Gender.CommonVerb; break;
                default:
                    throw new Exception(string.Format("Unexpected gender '{0}' in '{1}'", genderStr, str));
            }

            string numberStr = str.Substring(2, 2);
            switch (numberStr) {
                case "sg": number = Number.Singular; break;
                case "pl": number = Number.Plural; break;
                default:
                    throw new Exception(string.Format("Unexpected Number '{0}' in '{1}'", numberStr, str));
            }
        }

        private string GetValue(int row, int column) {
            CellIndex cellIndex = new CellIndex(row, column);
            CellSelection selection = _cells[cellIndex];
            ICellValue value = selection.GetValue().Value;
            CellValueFormat format = selection.GetFormat().Value;
            CellValueFormatResult formatResult = format.GetFormatResult(value);

            return formatResult.InfosText;
        }
    }
}
