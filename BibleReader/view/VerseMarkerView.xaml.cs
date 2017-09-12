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
    public partial class VerseMarkerView : UserControl {
        public VerseMarkerView() {
            InitializeComponent();
        }

        public void SetVerse(Verse verse) { 
            uxTbVerseMarker.Text = string.Format("{0}:{1}", verse.Chapter.Number, verse.Number);

            if (verse.IsParagraphStart) {
                uxTbVerseMarker.FontWeight = FontWeights.Bold;
                uxTbVerseMarker.Background = Brushes.LightSeaGreen;
            }
            else {
                uxTbVerseMarker.Background = Brushes.LightBlue;
            }
        }
    }
}
