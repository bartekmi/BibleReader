using BibleReader.model.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleReader.model {

    public class Bible {
        public Language Language { get; set; }
        public string Name { get; set; }
        public List<Book> Books { get; private set; }

        public bool IsRightToLeft { get { return Language == Language.Hebrew; } }

        internal Bible() {
            Books = new List<Book>();
        }

        public Book GetBook(string name) {
            return Books.SingleOrDefault(x => x.Name == name);
        }

        public override string ToString() {
            return Name;
        }
    }
}
