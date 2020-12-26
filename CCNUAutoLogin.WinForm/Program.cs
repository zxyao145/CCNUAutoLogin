using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
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
        static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Utils.RealStartupDir = AppContext.BaseDirectory;
            var fileName = Path.GetFileNameWithoutExtension(Application.ExecutablePath);
            Utils.ExecutablePath =  Path.Combine(Directory.GetCurrentDirectory(), fileName + ".exe");
           
            WindowsPrincipal winPrincipal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
            bool isAdmin = winPrincipal.IsInRole(WindowsBuiltInRole.Administrator);
            if (isAdmin)
            {
                bool autoStartup = false;
                if (args != null && args.Length > 0)
                {
                    autoStartup = Convert.ToBoolean(args[0]);
                }

                if (autoStartup)
                {
                    AutoStartup.Set(true);
                }
            }
            Application.Run(new MainForm());
        }
    }
}
