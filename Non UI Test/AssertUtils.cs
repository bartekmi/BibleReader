using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleReader {
    public static class AssertUtils {
        public static void AssertStringsEqual(string expected, string actual) {
            if (expected == actual)
                return;

            string[] expectedLines = expected.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            string[] actualLines = actual.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            int minLines = Math.Min(expectedLines.Length, actualLines.Length);

            for (int ii = 0; ii < minLines; ii++) {
                string expectedLine = expectedLines[ii];
                string actualLine = actualLines[ii];

                if (expectedLine == actualLine)
                    continue;

                int minChars = Math.Min(expectedLine.Length, actualLine.Length);
                for (int jj = 0; jj < minChars; jj++) {
                    if (expectedLine[jj] != actualLine[jj])
                        ThrowException(expected, actual, string.Format("Strings differ at Line {0}, Character {1}", ii + 1, jj + 1));
                }

                ThrowException(expected, actual, string.Format(
@"Strings differ at the end at Line {0} (one has {1} more characters).
    Expected: {2}
    Actual: {3}"
                    , ii + 1, Math.Abs(expectedLine.Length - actualLine.Length), expectedLine, actualLine));
            }

            ThrowException(expected, actual, string.Format(
@"Strings differ at the end (one has more lines)
    Expected: { 1 }
    Actual: { 2}"
                    , expected, actual));
        }


        private static void ThrowException(string expected, string actual, string additionalInfo) {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine("Expected:");
            builder.AppendLine(expected);
            builder.AppendLine();
            builder.AppendLine();
            builder.AppendLine("Actual:");
            builder.AppendLine(actual);
            builder.AppendLine();
            builder.AppendLine();
            builder.AppendLine(additionalInfo);

            throw new Exception(builder.ToString());
        }

    }
}
