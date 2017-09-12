using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleReader.model {

    public enum Punctuation {
        Maqqef,     // "Binder" - dash
        SofPasuq,   // Colon
        Samekh,
        Pe,         // End of Paragraph
        Paseq,
        ReversedNun,
    }

    public class ElementPunctuation : Element {

        public Punctuation Type { get; set; }

        public ElementPunctuation(Verse verse) : base(verse) {
            // Do nothing
        }
    }
}
