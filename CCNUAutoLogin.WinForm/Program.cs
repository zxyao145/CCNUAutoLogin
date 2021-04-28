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
            // 注册GB2312编码
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // 设置程序真实允许目录（单一程序会解压缩的目录），以写入配置文件
            Utils.RealStartupDir = AppContext.BaseDirectory;
            var fileName = Path.GetFileNameWithoutExtension(Application.ExecutablePath);
            // 获取当前程序（单一程序）执行路径（替换扩展名.dll为.exe），以便写入注册表
            Utils.ExecutablePath =  Path.Combine(Directory.GetCurrentDirectory(), fileName + ".exe");
           
            // 判断是否是管理员身份运行
            WindowsPrincipal winPrincipal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
            bool isAdmin = winPrincipal.IsInRole(WindowsBuiltInRole.Administrator);
            if (isAdmin)
            {
                // 以管理员身份运行，判断有没参数
                // 第一个参数表示是否允许开机自启
                if (args != null && args.Length > 0)
                {
                    var autoStartup = Convert.ToBoolean(args[0]);
                    AutoStartup.Set(autoStartup);
                    var config = new AppConfig
                    {
                        AutoStartup = autoStartup
                    };
                    var _ = AppConfigIO.Write<AppConfig>(config, "app.config");
                }
            }

            // 程序运行主窗体
            try
            {
                Application.Run(new MainForm());
            }
            catch (Exception e)
            {
                LogHelper.WriteError("程序发生致命错误：" + e);
                Utils.ReStart();
            }
        }
    }
}
