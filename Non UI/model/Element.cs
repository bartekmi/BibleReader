using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleReader.model {
    public abstract class Element {
        public string Text { get; set; }
        public Verse Verse { get; private set; }

        // Derived
        public bool IsFirstInVerse { get { return Verse.Words.First() == this; } }
        public bool IsLastInVerse { get { return Verse.Words.Last() == this; } }

        protected Element(Verse verse) {
            Verse = verse;
        }

        public override string ToString() {
            return Text;
        }
    }
}
