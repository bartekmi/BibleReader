using BibleReader.helpers;
using BibleReader.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleReader.View {
    internal class HoverViewVM : NotificationObject {

        public ElementWord Word { get; private set; }
        public LexiconWordDefinition Definition { get; private set; }

        internal HoverViewVM(LexiconWordDefinition definition) {
            Definition = definition;
        }

        internal HoverViewVM(ElementWord word) {
            Word = word;
            Definition = word.Definitions.First();
        }
    }
}
