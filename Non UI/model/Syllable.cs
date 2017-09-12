using BibleReader.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleReader.model {

    public enum SyllableType {
        Open,
        Closed,
        HolemVav,
    }

    public class Syllable {
        public Letter[] Letters { get; private set; }
        public SyllableType Type { get; private set; }

        // Derived
        bool IsClosed { get { return Type == SyllableType.Closed; } }
        bool IsOpen { get { return Type == SyllableType.Open; } }

        public Syllable(string letters, SyllableType type) {
            Letters = HebrewTextConversionUtils.Extract(letters);
            Type = type;
        }

        public Syllable(Letter[] letters, SyllableType type) {
            Letters = letters;
            Type = type;
        }

        public override string ToString() {
            StringBuilder builder = new StringBuilder();

            foreach (Letter letter in Letters) 
                builder.Append((char)letter.TheLetter);

            builder.Append(string.Format(" ({0})", Type));

            return builder.ToString();
        }
    }
}
