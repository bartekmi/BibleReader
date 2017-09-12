using BibleReader.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleReader.DataSource
{
    public abstract class LexiconSource : Source
    {
        public abstract Lexicon HydrateLexicon();
    }
}
