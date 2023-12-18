using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LUMINET
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            Application.Run(new LUMINET());
        }

        static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            MessageBox.Show("There was an issue performing a required action. Don't worry, this isn't your fault. Please report this issue to our GitHub — It will automatically be opened on the closure of this message.", "Oopsie!", MessageBoxButtons.OK, MessageBoxIcon.Error);

            File.WriteAllText(Application.ExecutablePath + "\\error-log.txt", e.Exception.Message);

            Process.Start("https://github.com/voidZiAD/LUMINET/issues");

            MessageBox.Show("We've made it easier for you:", "Please open the folder of where the VPN .exe is present, you will find a file called 'error-log.txt', mention whatever is included inside of it on the GitHub issue that you create. Thank you, and we're sorry for the inconvenience.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show("There was an issue performing a required action. Don't worry, this isn't your fault. Please report this issue to our GitHub — It will automatically be opened on the closure of this message.", "Oopsie!", MessageBoxButtons.OK, MessageBoxIcon.Error);

            File.WriteAllText(Application.ExecutablePath + "\\error-log.txt", (e.ExceptionObject as Exception).Message);

            Process.Start("https://github.com/voidZiAD/LUMINET/issues");

            MessageBox.Show("We've made it easier for you:", "Please open the folder of where the VPN .exe is present, you will find a file called 'error-log.txt', mention whatever is included inside of it on the GitHub issue that you create. Thank you, and we're sorry for the inconvenience.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
    }
}
