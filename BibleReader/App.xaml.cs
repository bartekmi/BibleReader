using BibleReader.View;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace BibleReader
{

    public partial class App : Application {
        private void ApplicationStart(object sender, StartupEventArgs e) {
            new AppStatics();
            Current.MainWindow = new MainWindow();
            Current.MainWindow.Show();
            AppStatics.Singleton.IsStartupMode = false;
        }
    }
}
