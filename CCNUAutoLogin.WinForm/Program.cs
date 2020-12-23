using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using CCNUAutoLogin.Core;

namespace CCNUAutoLogin.WinForm
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //AutoStartup.SetStartupPath(Application.StartupPath);
            Utils.ExecutablePath = Application.ExecutablePath;
            Application.Run(new MainForm());
        }
    }
}
