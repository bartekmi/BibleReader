using BibleReader.model.enums;
using System.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleReader.model.conjugation {
    public abstract class ConjugationBase {

        public PartOfSpeech PartOfSpeech { get; private set; }

        protected ConjugationBase(PartOfSpeech partOfSpeech) {
            PartOfSpeech = partOfSpeech;
        }

        public override string ToString() {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine(string.Format("Part of Speech: {0}", PartOfSpeech));

            foreach (PropertyInfo property in this.GetType().GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)) {
                object value = property.GetValue(this);
                if (value != null)
                    builder.AppendLine(string.Format("{0}: {1}", property.Name, value));
            }

            return builder.ToString().Trim();
        }
    }
}
