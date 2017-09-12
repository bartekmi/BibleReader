using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleReader.model
{
    public class Chapter {
        public int Number { get; set; }

        public Book Book { get; private set; }
        public List<Verse> Verses { get; private set; }
        public int Index { get; set; }

        internal Chapter(Book book) {
            Book = book;
            Verses = new List<Verse>();
            Index = book.Chapters.Count;
        }

        public void PopulateOtherBibleVersions() {
            string[] verses = AppStaticsNonUI.Singleton.EsvApi.GetChapter(this);
            if (verses == null)
                return;

            for (int ii = 0; ii < verses.Length && ii < Verses.Count; ii++)
                Verses[ii].TextOfVerseEsv = verses[ii];
        }

        public void GetNext(out Book book, out Chapter chapter)
        {
            if (IsLast)
            {
                book = Book.Bible.Books[Book.Index + 1];
                chapter = book.Chapters.First();
            }
            else
            {
                book = Book;
                chapter = Book.Chapters[Index + 1];
            }
        }

        public void GetPrevious(out Book book, out Chapter chapter)
        {
            if (IsFirst)
            {
                book = Book.Bible.Books[Book.Index - 1];
                chapter = book.Chapters.Last();
            }
            else
            {
                book = Book;
                chapter = Book.Chapters[Index - 1];
            }
        }

        public bool IsLast
        {
            get { return this == this.Book.Chapters.Last(); }
        }

        public bool IsFirst
        {
            get { return this == this.Book.Chapters.First(); }
        }

        public override string ToString() {
            return Number.ToString();
        }

        public override bool Equals(object obj) {
            if (!(obj is Chapter))
                return false;

            return Index == (obj as Chapter).Index;
        }

        public override int GetHashCode() {
            return Index;
        }
    }
}
