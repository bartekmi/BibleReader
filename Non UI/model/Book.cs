using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleReader.model
{
    public enum BookEnum {
        Gen,
        Exo,
        Lev,
        Num,
        Deu,
        Jos,
        Jdg,
        Rut,
        _1Sa,
        _2Sa,
        _1Ki,
        _2Ki,
        _1Ch,
        _2Ch,
        Ezr,
        Neh,
        Est,
        Job,
        Psa,
        Pro,
        Ecc,
        Sng,
        Isa,
        Jer,
        Lam,
        Ezk,
        Dan,
        Hos,
        Jol,
        Amo,
        Oba,
        Jon,
        Mic,
        Nam,
        Hab,
        Zep,
        Hag,
        Zec,
        Mal,
        Tob,
        Jdt,
        Esg,
        Wis,
        Sir,
        Bar,
        S3y,
        Sus,
        Bel,
        _1Ma,
        _2Ma,
        _1Es,
        Man,
        _2Es,
        Mat,
        Mrk,
        Luk,
        Jhn,
        Act,
        Rom,
        _1Co,
        _2Co,
        Gal,
        Eph,
        Php,
        Col,
        _1Th,
        _2Th,
        _1Ti,
        _2Ti,
        Tit,
        Phm,
        Heb,
        Jas,
        _1Pe,
        _2Pe,
        _1Jn,
        _2Jn,
        _3Jn,
        Jud,
        Rev,
    }

    public class Book
    {
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public BookEnum BookEnum { get; set; }
        public int Index { get; set; }

        public Bible Bible { get; private set; }
        public virtual List<Chapter> Chapters { get; private set; }

        internal Book(Bible bible)
        {
            Bible = bible;
            Chapters = new List<Chapter>();
            Index = bible.Books.Count;
        }

        public Chapter GetChapter(int number) {
            return Chapters.SingleOrDefault(x => x.Number == number);
        }

        public override bool Equals(object obj) {
            if (!(obj is Book))
                return false;

            Book other = obj as Book;
            return BookEnum == other.BookEnum;
        }

        public override int GetHashCode() {
            return (int)BookEnum;
        }
        // Used in GUI
        public override string ToString()
        {
            return Abbreviation;
        }
    }
}
