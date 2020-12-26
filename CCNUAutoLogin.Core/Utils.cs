using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Mime;
using System.Text;

namespace CCNUAutoLogin.Core
{
    public class Utils
    {
        public static string RealStartupDir { get; set; }

        public static string ExecutablePath { get; set; }
        public static int RunAsAdmin(string arguments = "")
        {
            Process process = null;
            ProcessStartInfo processInfo = new ProcessStartInfo
            {
                Verb = "runas", 
                FileName = ExecutablePath,
                Arguments = arguments,
                UseShellExecute = true
            };
            try
            {
                process = Process.Start(processInfo);
            }
            catch (System.ComponentModel.Win32Exception)
            {
                return -1;
            }

            if (process != null)
            {
                process.Close();
                return 0;
            }
            return -1;
        }
    }
}
