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
using System.Xml;
using BibleReader.model.enums;

namespace BibleReader.View {
    public partial class HoverView : UserControl {
        public HoverView() {
            InitializeComponent();
            MainWindow.SetFont(uxTbOriginalLanguage);

            uxLvReferences.SelectionChanged += (s, e) => {
                ElementWord word = (ElementWord)uxLvReferences.SelectedItem;
                if (word != null)
                    AppStatics.Singleton.MainWindow.GoToVerse(word.Verse);
            };
        }

        public HoverView(ElementWord word) : this() {
            SetDefinition(word);
        }

        internal void SetDefinition(ElementWord word) {
            DataContext = new HoverViewVM(word);
            PopulateExplanation(word.Definitions.FirstOrDefault());
        }

        internal void SetDefinition(LexiconWordDefinition definition) {
            DataContext = new HoverViewVM(definition);
            PopulateExplanation(definition);
        }

        private void PopulateExplanation(LexiconWordDefinition definition) {

            uxTbExplanation.Inlines.Clear();
            if (definition == null)
                return;

            foreach (XmlNode child in definition.Explanation.ChildNodes) {
                Run run = new Run(child.InnerText);

                if (child.Name == "hi")
                    uxTbExplanation.Inlines.Add(new Bold(run));
                else
                    uxTbExplanation.Inlines.Add(run);
            }
        }
    }
}
