using BibleReader.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleReader.DataSource
{
    public abstract class BibleSource : Source
    {
        public abstract Bible HydrateBible();
    }
}
