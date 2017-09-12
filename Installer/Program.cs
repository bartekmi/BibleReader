using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WITS_Installer {
    class Program {
        private const string BUILD_FILE_ORIGINAL = @"..\..\BibleReader.aip";
        private const string BUILD_FILE_MODIFIED = @"..\..\BibleReader_TempFilePayNoAttentionToMe.aip";
        private const string MSI_FILE_DIR = @"..\..\Setup Files";
        private const string MSI_FILE_NAME = @"BibleReader_TempFilePayNoAttentionToMe.msi";

        static void Main(string[] args) {
            try {
                ReplaceProductVersion();
                BuildInstaller();
                RenameMsi();

                Console.WriteLine("Success.");
            } catch (Exception e) {
                Console.WriteLine("******************************   ERROR  ***************************************");
                Console.WriteLine(e);
            }

            Console.WriteLine("Press enter to Exit");
            Console.ReadLine();
        }

        private static string ProductVersion() {
            string version = File.ReadAllText("Version.txt").Trim();
            return version;
        }

        private static void ReplaceProductVersion() {
            Console.WriteLine("Setting Product Version: " + ProductVersion());
            File.Copy(BUILD_FILE_ORIGINAL, BUILD_FILE_MODIFIED, true);

            RunAdvancedInstallerCommand("/edit {0} /SetVersion " + ProductVersion());
            RunAdvancedInstallerCommand("/edit {0} /SetProductCode -langid 1033 -guid {{" + Guid.NewGuid().ToString() + "}}");       // Double braces escape later use of string.format()
        }

        private static void BuildInstaller() {
            RunAdvancedInstallerCommand("/rebuild {0}");
        }

        private static void RenameMsi() {
            string oldName = Path.Combine(MSI_FILE_DIR, MSI_FILE_NAME);
            string newName = string.Format(@"{0}\BibleReader_{1}.msi", MSI_FILE_DIR, ProductVersion());
            if (File.Exists(newName))
                File.Delete(newName);
            File.Move(oldName, newName);
        }

        private static void RunAdvancedInstallerCommand(string parameters) {
            parameters = string.Format(parameters, BUILD_FILE_MODIFIED);
            string builder = @"C:\Program Files (x86)\Caphyon\Advanced Installer 13.4\bin\x86\AdvancedInstaller.com";
            Console.WriteLine(string.Format("\"{0}\" {1}", builder, parameters));

            Process p = new Process();
            p.StartInfo.FileName = builder;
            p.StartInfo.Arguments = parameters;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.Start();

            string output = p.StandardOutput.ReadToEnd();
            Console.WriteLine(output);
            p.WaitForExit();
            if (p.ExitCode != 0)
                throw new Exception("Build Error: " + p.ExitCode);
        }
    }
}
