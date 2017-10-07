using BibleReader.DataSource;
using BibleReader.DataSource.os;
using BibleReader.helpers;
using BibleReader.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleReader.View {
    internal class MainWindowVM : NotificationObject {

        private MainWindow _view;

        public List<Bible> Bibles { get; private set; }
        public List<Book> Books { get; private set; }
        public List<Chapter> Chapters { get; private set; }
        public List<Verse> Verses { get; private set; }

        private Bible _selectedBible;
        public Bible SelectedBible {
            get { return _selectedBible; }
            set {
                _selectedBible = value;

                Book previousBook = SelectedBook;
                Chapter previousChapter = SelectedChapter;

                Books = value.Books;

                RaisePropertyChanged(() => Books);
                RaisePropertyChanged(() => SelectedBible);

                if (previousBook != null) {
                    SelectedBook = Books.SingleOrDefault(x => x.Equals(previousBook));
                    if (SelectedBook == null) {
                        SelectedBook = Books.First();
                        AppStatics.Singleton.ShowMessage(string.Format("{0} not present in {1}", previousBook.Name, _selectedBible.Name));
                        return;
                    }

                    SelectedChapter = SelectedBook.Chapters.SingleOrDefault(x => x.Equals(previousChapter));
                    if (SelectedChapter == null)
                        SelectedChapter = SelectedBook.Chapters.First();
                }
            }
        }

        private Book _selectedBook;
        public Book SelectedBook {
            get { return _selectedBook; }
            set {
                if (value == null || value == _selectedBook)
                    return;

                _selectedBook = value;
                Chapters = value.Chapters;
                SelectedChapter = Chapters.First();

                RaisePropertyChanged(() => Chapters);
                RaisePropertyChanged(() => SelectedBook);
            }
        }

        private Chapter _selectedChapter;
        public Chapter SelectedChapter {
            get { return _selectedChapter; }
            set {
                if (value == null || value == _selectedChapter)
                    return;

                _selectedChapter = value;
                Verses = value.Verses;

                RaisePropertyChanged(() => Verses);
                RaisePropertyChanged(() => SelectedChapter);

                _view.DisplayChapter(value);

                if (!AppStatics.Singleton.IsStartupMode)
                    AppStatics.Singleton.AppState.Save(value);
            }
        }


        internal MainWindowVM(MainWindow view) {
            _view = view;
            _view.DataContext = this;       // Otherwise, it's too late

            Bibles = new List<Bible>() {
                new BibleSourceKjvUsfx().HydrateBible(),
                new BibleSourceOsWlc().HydrateBible(),
            };


            Lexicon lexicon = new LexiconSourceStrongs().HydrateLexicon();
            lexicon.GenerateIndex(Bibles[0]);       // Index should be associated with the Bible, not the Lexicon
            AppStatics.Singleton.CurrentLexison = lexicon;

            // Restore saved state
            SelectedBible = Bibles.SingleOrDefault(x => x.Name == AppStatics.Singleton.AppState.Bible);
            Book savedBook = SelectedBible.GetBook(AppStatics.Singleton.AppState.Book);
            if (savedBook == null)
                SelectedBook = Books.First();
            else {
                SelectedBook = savedBook;
                Chapter savedChapter = savedBook.GetChapter(AppStatics.Singleton.AppState.Chapter);
                if (savedChapter == null)
                    SelectedChapter = Chapters.First();
                else
                    SelectedChapter = savedChapter;
            }
        }

        internal void AdvanceChapter(bool forward) {
            Book book;
            Chapter chapter;
            if (forward)
                SelectedChapter.GetNext(out book, out chapter);
            else
                SelectedChapter.GetPrevious(out book, out chapter);

            SelectedBook = book;
            SelectedChapter = chapter;
        }
    }
}
