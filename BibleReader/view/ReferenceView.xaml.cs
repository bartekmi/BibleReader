using BibleReader.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BibleReader.View {
    public partial class ReferenceView : UserControl {

        private const int NUM_BEFORE_AFTER = 5;

        public ReferenceView() {
            InitializeComponent();
        }

        private string PartialVerse(Verse verse, int from, int to) {
            from = Limit(verse, from);
            to = Limit(verse, to);

            return string.Join(" ", verse.Words.Skip(from).Take(to - from + 1).Select(x => x.Text));
        }

        private int Limit(Verse verse, int index) {
            if (index < 0)
                index = 0;

            if (index >= verse.Words.Count)
                index = verse.Words.Count - 1;

            return index;
        }
    }
}
