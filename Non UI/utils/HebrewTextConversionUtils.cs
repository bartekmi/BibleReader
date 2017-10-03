using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BibleReader.model;
using BibleReader.model.enums;

namespace BibleReader.utils {

    public static class HebrewTextConversionUtils {

        public static bool IsVowelPoint(HAnnotation annotation) {
            return annotation >= HAnnotation.PointSheva && annotation <= HAnnotation.PointQubuts;
        }

        public static bool IsSeparator(char c) {
            return c == '/';
        }

        public static bool IsLetter(char c) {
            return c >= (char)HLetter.Aleph && c <= (char)HLetter.Tav;
        }

        public static bool IsLetterPointCombo(char c) {
            return c >= (char)HLetterPointCombo.YodWithHiriq && c <= (char)HLetterPointCombo.PeWithRafe;
        }

        public static bool IsAnnotation(char c) {
            return Enum.GetValues(typeof(HAnnotation)).Cast<int>().Contains(c);
        }

        #region Extract
        public static Letter[] Extract(string word) {
            if (word == null || word.Length == 0)
                throw new Exception("No input to Extract()");

            List<Letter> letters = new List<Letter>();
            List<HAnnotation> annotations = new List<HAnnotation>();
            HLetter? letter = null;

            foreach (char c in word) {
                Console.WriteLine(((int)c).ToString("X"));

                if (IsLetter(c) || IsSeparator(c)) {
                    if (letter != null)
                        ComposeLetter(letters, letter, annotations);
                    letter = (HLetter)c;
                } else if (IsLetterPointCombo(c)) {
                    if (letter != null)
                        ComposeLetter(letters, letter, annotations);
                    letter = HLetterPointComboHelper.Normalize((HLetterPointCombo)c, annotations);
                } else if (IsAnnotation(c))
                    annotations.Add((HAnnotation)c);
                else
                    throw new Exception(string.Format("Character '{0}' ({1}) is neither a Hebrew letter nor a Hebrew annotation", c, ((int)c).ToString("X")));
            }

            ComposeLetter(letters, letter, annotations);

            return letters.ToArray();
        }

        private static void ComposeLetter(List<Letter> letters, HLetter? letter, List<HAnnotation> annotations) {
            letters.Add(new Letter(letter.Value, annotations.ToArray()));
            annotations.Clear();
        }
        #endregion

        #region Other Methods
        internal static bool AreSame(Letter[] a, Letter[] b, bool isStrict) {
            if (a.Length != b.Length)
                return false;

            for (int ii = 0; ii < a.Length; ii++)
                if (!a[ii].Equals(b[ii], isStrict))
                    return false;

            return true;
        }
        #endregion
    }
}
