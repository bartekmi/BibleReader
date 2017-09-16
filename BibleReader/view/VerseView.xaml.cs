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
    public partial class VerseView : UserControl {
        public VerseView() {
            InitializeComponent();
        }

        public VerseView(model.Verse verse) : this() {
            DataContext = verse;
            uxVerseMarkerView.SetVerse(verse);

            IEnumerable<Element> elements = verse.Words;
            uxWrapPanel.FlowDirection = AppStatics.Singleton.Bible.IsRightToLeft ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;

            foreach (Element element in elements) {
                WordView wordView = element is ElementWord ?
                    new WordView(element as ElementWord) :
                    new WordView(element as ElementPunctuation);
                uxWrapPanel.Children.Add(wordView);
            }
        }
    }
}
