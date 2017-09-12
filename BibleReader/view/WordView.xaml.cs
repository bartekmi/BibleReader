using BibleReader.behaviors;
using BibleReader.converter;
using BibleReader.grammar;
using BibleReader.model;
using BibleReader.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BibleReader.View {
    public partial class WordView : UserControl {
        public static readonly Thickness MARGIN_WORD = new Thickness(10, 0, 0, 0);

        private Element ViewModel { get { return (Element)DataContext; } }
        public bool IsBinder {
            get {
                return ViewModel is ElementPunctuation && (ViewModel as ElementPunctuation).Type == Punctuation.Maqqef;
            }
        }

        public WordView() {
            InitializeComponent();

            bool isBibleEnglish = AppStatics.Singleton.Bible.Language == model.enums.Language.English;
            FrameworkElement toBeMagnified = isBibleEnglish ? (FrameworkElement)uxSpWordDetails : uxTbTheWord;

            Binding binding = new Binding() {
                RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(Grid), 1),
                Path = new PropertyPath(TextBlock.FontSizeProperty),
                Converter = new ScaleConverter(),
                ConverterParameter = 1.5,
            };

            toBeMagnified.SetBinding(TextBlock.FontSizeProperty, binding);
        }

        public WordView(ElementWord word)
            : this() {
            DataContext = word;

            if (word.IsAddedByTranslators)
                uxTbTheWord.FontStyle = FontStyles.Italic;

            if (word.Verse.Chapter.Book.Bible.Language == model.enums.Language.Hebrew) {
                PopulateTransliteration(word);
                PopupBehavior.Attach(uxTbTheWord, () => new HoverView(word));
                uxTbTheWord.MouseDown += (s, e) => AppStatics.Singleton.MainWindow.SelectWord(word.Definitions.FirstOrDefault());
            } else {
                if (word.StrongsNumbers != null && word.StrongsNumbers.Length > 0)
                    PopulateOriginalWords(word);
                else
                    uxSpWordDetails.Children.Add(new TextBlock() { Text = " " });  // Spacer to keep things aligned vertically
            }
        }

        public WordView(ElementPunctuation punctuation)
            : this() {
            DataContext = punctuation;
            uxSpWordDetails.Visibility = Visibility.Collapsed;
            //uxSpWordDetails.Children.Add(new TextBlock() { Text = " " });  // Spacer to keep things aligned vertically
        }

        private void PopulateTransliteration(ElementWord word) {
            Letter[] letters = HebrewTextConversionUtils.Extract(word.Text);
            string transliteration = new Transliterator().Transliterate(letters);
            TextBlock textBlock = new TextBlock() {
                Text = transliteration,
            };
            uxSpWordDetails.Children.Add(textBlock);
        }

        private void PopulateOriginalWords(ElementWord word) {
            bool first = true;
            foreach (string strongs in word.StrongsNumbers) {
                LexiconWordDefinition definition = AppStatics.Singleton.CurrentLexison.LookUp(strongs);
                if (definition != null) {
                    string spacer = first ? "" : " ";
                    TextBlock originalLanuageWord = new TextBlock() {
                        Text = definition.OriginalLanguage + spacer,
                    };

                    PopupBehavior.Attach(originalLanuageWord, () => new HoverView(word));
                    originalLanuageWord.MouseDown += (s, e) => AppStatics.Singleton.MainWindow.SelectWord(definition);

                    if (definition.FirstReference() == ViewModel)
                        originalLanuageWord.Background = Brushes.Yellow;

                    first = false;
                    uxSpWordDetails.Children.Insert(0, originalLanuageWord);       // Insert because Hebrew is back-to-front
                }
            }
        }

        public override string ToString() {
            return ViewModel.Text;
        }
    }
}
