using BibleReader.DataSource;
using BibleReader.DataSource.os;
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
using System.Windows.Threading;

namespace BibleReader.View {
    public partial class MainWindow : Window {

        internal MainWindowVM ViewModel { get { return (MainWindowVM)DataContext; } }

        public MainWindow() {
            InitializeComponent();
            AppStatics.Singleton.MainWindow = this;
            DataContext = new MainWindowVM(this);

            SetFont(uxStackPanel);
            ToolTipService.ShowDurationProperty.OverrideMetadata(typeof(DependencyObject), new FrameworkPropertyMetadata(Int32.MaxValue));

            PreviewMouseWheel += (s, e) => Zoom(e.Delta, e);
            PreviewKeyDown += (s, e) => KeyDownActions(e);
            uxBtNext.Click += (s, e) => ViewModel.AdvanceChapter(true);
            uxBtPrevious.Click += (s, e) => ViewModel.AdvanceChapter(false);
        }

        #region Word Selection

        internal void SelectWord(LexiconWordDefinition definition) {
            uxHoverViewSelectedWord.Visibility = Visibility.Visible;
            uxHoverViewSelectedWord.SetDefinition(definition);
        }

        #endregion

        #region Zoom
        private void KeyDownActions(KeyEventArgs e) {
            switch (e.Key) {
                case Key.OemPlus:
                    Zoom(+1, e);
                    break;
                case Key.OemMinus:
                    Zoom(-1, e);
                    break;
            }
        }

        private void Zoom(int delta, RoutedEventArgs e) {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)) {
                double fontSize = TextBlock.GetFontSize(uxStackPanel);
                double factor = 1.2;
                if (delta < 0)
                    factor = 1.0 / factor;

                fontSize *= factor;
                fontSize = Math.Max(fontSize, 8.0);
                fontSize = Math.Min(fontSize, 30.0);

                TextBlock.SetFontSize(uxStackPanel, fontSize);
                SetSpacingBasedOnFontSize();

                e.Handled = true;
            }
        }

        private double? _nominalFontSize;
        private void SetSpacingBasedOnFontSize() {
            double fontSize = TextBlock.GetFontSize(uxStackPanel);
            if (_nominalFontSize == null)
                _nominalFontSize = fontSize;
            double ratio = fontSize / _nominalFontSize.Value;

            foreach (VerseView panel in uxStackPanel.Children) {
                panel.Margin = new Thickness(0, 0, 0, 15 * ratio);
                List<WordView> wordViews = panel.uxWrapPanel.Children.OfType<WordView>().ToList();
                for (int ii = 0; ii < wordViews.Count; ii++) {
                    WordView word = wordViews[ii];
                    if (word.IsBinder || ii > 0 && wordViews[ii - 1].IsBinder)
                        word.Margin = new Thickness(0);
                    else
                        word.Margin = ScaleMargin(WordView.MARGIN_WORD, ratio);
                }
            }
        }

        private Thickness ScaleMargin(Thickness margin, double ratio) {
            return new Thickness(margin.Left * ratio, margin.Top * ratio, margin.Right * ratio, margin.Bottom * ratio);
        }
        #endregion

        #region Display

        internal void DisplayChapter(Chapter chapter) {
            uxStackPanel.Children.Clear();

            foreach (Verse verse in chapter.Verses) {
                VerseView verseView = new VerseView(verse);
                uxStackPanel.Children.Add(verseView);
            }

            SetSpacingBasedOnFontSize();
            uxScrollViewer.ScrollToHome();
            uxScrollViewer.Focus();
        }


        internal void GoToVerse(Verse verse) {
            ViewModel.SelectedBook = verse.Chapter.Book;
            ViewModel.SelectedChapter = verse.Chapter;

            // Scroll Verse into View
            Dispatcher.Invoke(() => { }, DispatcherPriority.Render);        // Forces Layout so that TranslatePoint later actually works
            VerseView wrap = uxStackPanel.Children.Cast<VerseView>().Single(x => x.DataContext == verse);
            FrameworkElement content = (FrameworkElement)uxScrollViewer.Content;
            Point point = wrap.TranslatePoint(new Point(0, 0), content);
            uxScrollViewer.ScrollToVerticalOffset(point.Y);
        }

        #endregion

        #region Font
        public static void SetFont(FrameworkElement element) {
            // In future, maybe user can select font
            FontFamily family = Fonts.GetFontFamilies(new Uri("pack://application:,,,/font/#")).FirstOrDefault();
            if (family == null)
                return;
            TextBlock.SetFontFamily(element, family);
        }
        #endregion
    }
}
