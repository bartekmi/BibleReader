using BibleReader.apis;
using BibleReader.model;
using BibleReader.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BibleReader {
    public class AppStatics : AppStaticsNonUI {
        public static new AppStatics Singleton { get; private set; }

        public MainWindow MainWindow { get; set; }
        public Bible Bible { get { return MainWindow.ViewModel.SelectedBible; } }

        internal bool IsStartupMode { get; set; }

        public AppStatics() {
            Singleton = this;
            IsStartupMode = true;
        }

        public void ShowMessage(string message) {
            MessageBox.Show(message);
        }
    }
}
