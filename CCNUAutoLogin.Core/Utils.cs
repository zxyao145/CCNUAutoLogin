using System.Diagnostics;

namespace CCNUAutoLogin.Core
{
    public class Utils
    {
        /// <summary>
        /// 程序真实允许目录，单一程序会进行解压缩
        /// Windows下一般是%temp%/.net/CCNUAutoLogin/{hash}
        /// </summary>
        public static string RealStartupDir { get; set; }

        /// <summary>
        /// 程序（单一程序）执行路径
        /// </summary>
        public static string ExecutablePath { get; set; }

        /// <summary>
        /// 以管理员身份运行
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 以管理员身份运行
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public static int ReStart(string arguments = "")
        {
            Process process = null;
            ProcessStartInfo processInfo = new ProcessStartInfo
            {
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
