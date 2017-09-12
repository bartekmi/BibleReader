using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace BibleReader.model {
    public class ApplicationState {

        private const string FILENAME = @".\BibleReaderState.json";

        public string Bible;
        public string Book;
        public int Chapter;
        public int Verse;

        public void Save(Chapter chapter) {
            Bible = chapter.Book.Bible.Name;
            Book = chapter.Book.Name;
            Chapter = chapter.Number;
            Save();
        }

        internal void Save() {
            string json = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(FILENAME, json);
        }

        internal static ApplicationState LoadOrCreate() {
            if (File.Exists(FILENAME))
                try {
                    string json = File.ReadAllText(FILENAME);
                    return JsonConvert.DeserializeObject<ApplicationState>(json);
                } catch {
                    // Simply fail and go on...
                }

            return new ApplicationState() {
                Bible = "King James Version",
                Book = "GEN",
                Chapter = 1,
            };
        }
    }
}
