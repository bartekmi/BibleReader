using BibleReader.apis;
using BibleReader.DataSource;
using BibleReader.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleReader {
    public class AppStaticsNonUI {
        public static AppStaticsNonUI Singleton { get; private set; }

        public Lexicon CurrentLexison { get; set; }
        public EsvApi EsvApi { get; private set; }
        public ApplicationState AppState { get; private set; }

        private Conjugations _conjugations;
        public Conjugations Conjugations {
            get {
                if (_conjugations == null)
                    _conjugations = new Conjugations();
                return _conjugations;
            }
        }

        public AppStaticsNonUI() {
            EsvApi = new EsvApi();
            AppState = ApplicationState.LoadOrCreate();
            Singleton = this;
        }

    }
}
