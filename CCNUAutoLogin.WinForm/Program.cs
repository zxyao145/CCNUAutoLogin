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
            // ע��GB2312����
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // ���ó�����ʵ����Ŀ¼����һ������ѹ����Ŀ¼������д�������ļ�
            Utils.RealStartupDir = AppContext.BaseDirectory;
            var fileName = Path.GetFileNameWithoutExtension(Application.ExecutablePath);
            // ��ȡ��ǰ���򣨵�һ����ִ��·�����滻��չ��.dllΪ.exe�����Ա�д��ע���
            Utils.ExecutablePath =  Path.Combine(Directory.GetCurrentDirectory(), fileName + ".exe");
           
            // �ж��Ƿ��ǹ���Ա�������
            WindowsPrincipal winPrincipal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
            bool isAdmin = winPrincipal.IsInRole(WindowsBuiltInRole.Administrator);
            if (isAdmin)
            {
                // �Թ���Ա������У��ж���û����
                // ��һ��������ʾ�Ƿ�����������
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

            // ��������������
            try
            {
                Application.Run(new MainForm());
            }
            catch (Exception e)
            {
                LogHelper.WriteError("��������������" + e);
                Utils.ReStart();
            }
        }
    }
}
