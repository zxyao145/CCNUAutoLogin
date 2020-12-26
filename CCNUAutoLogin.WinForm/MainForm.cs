using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CCNUAutoLogin.Core;

namespace CCNUAutoLogin.WinForm
{
    public partial class MainForm : Form
    {
        private AutoLoginService _autoLoginService;
        public MainForm()
        {
            InitializeComponent();
            this.password.PasswordChar = '*';
            this.ShowInTaskbar = true;
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            var config = FillUiByConfig();
            if (config == null)
            {
                this.Show();
                this.WindowState = FormWindowState.Normal;
                this.Activate();
            }

            _autoLoginService?.Start();

            configApp.Click += ConfigApp_Click;
            exitApp.Click += ExitApp_Click;
        }

        private void ExitApp_Click(object sender, EventArgs e)
        {
            this.Activate();
            //右键退出事件
            if (MessageBox.Show("是否需要关闭程序？", "提示:", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)//出错提示
            {
                //关闭窗口
                DialogResult = DialogResult.No;
                Dispose();
                Close();
            }
        }

        private void ConfigApp_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Show();
                this.WindowState = FormWindowState.Normal;
                this.Activate();
            }
        }

        private string _schNetType = "联通";
        private string _connectType = "无线";
        private bool _disableBtnSave = true;

        private readonly List<string> _schNetTypes = new List<string>()
        {
            "校园网","联通","移动","电信"
        };
        private readonly List<string> _connectTypes = new List<string>()
        {
            "有线","无线"
        };

        private AppConfig FillUiByConfig()
        {
            var config = AppConfigIO.Read();
            if (config != null)
            {
                _autoLoginService = new AutoLoginService(config);
                schoolNum.Text = config.SchNum;
                password.Text = config.Password;
                foreach (Control control in netTypePanel.Controls)
                {
                    if (control is RadioButton rBtn)
                    {
                        if (rBtn.Text == config.SchNetType)
                        {
                            rBtn.Checked = true;
                            break;
                        }
                    }
                }

                foreach (Control control in connectTypePanel.Controls)
                {
                    if (control is RadioButton rBtn)
                    {
                        if (rBtn.Text == config.SchNetType)
                        {
                            rBtn.Checked = true;
                            break;
                        }
                    }
                }
            }

            return config;
        }

        private void appNotifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)//判断鼠标的按键
            {
                //点击时判断form是否显示,显示就隐藏,隐藏就显示
                TriggerShowOrHide();
            }
        }

        private void OnNetTypeChanged(object sender, EventArgs e)
        {
            if (sender is RadioButton rtb)
            {
                _schNetType = rtb.Text;
            }
        }

        private void OnLanOrWlanTypeChanged(object sender, EventArgs e)
        {
            if (sender is RadioButton rtb)
            {
                _connectType = rtb.Text;
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            var config = new AppConfig()
            {
                SchNum = schoolNum.Text,
                Password = password.Text,
                ConnectType = _connectType,
                SchNetType = _schNetType
            };

            if (Verify(config))
            {
                var configPath = AppConfigIO.Write(config);

                if (_autoLoginService == null)
                {
                    _autoLoginService = new AutoLoginService(config);
                    _autoLoginService?.Start();
                }
                else
                {
                    _autoLoginService.UpdateConfig(config);
                }
                MessageBox.Show($"信息保存成功！{Environment.NewLine} {configPath}", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (AutoStartup.Set(true))
                {
                    TriggerShowOrHide();
                }
                else
                {
                    var dialogResult = MessageBox.Show("开机自启动需要管理员的权限，是否以管理员权限运行？", "提示", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        Utils.RunAsAdmin("true");
                        Dispose();
                        Close();
                    }
                }
            }
        }

        /// <summary>
        /// 验证config是否有效
        /// </summary>
        /// <param name="config"></param>
        /// <returns>true：有效；false：无效</returns>
        private bool Verify(AppConfig config)
        {
            var messages = new List<string>();
            if (config.SchNum.IsNullOrWhiteSpace())
            {
                messages.Add("用户名不能为空！");
            }
            if (config.Password.IsNullOrWhiteSpace())
            {
                messages.Add("密码不能为空！");
            }

            if (!_schNetTypes.Contains(config.SchNetType))
            {
                messages.Add("校园网网络类型错误！");
            }

            if (!_connectTypes.Contains(config.ConnectType))
            {
                messages.Add("校园网连接类型错误！");
            }

            if (messages.Count > 0)
            {
                var msg = string.Join(Environment.NewLine, messages);
                MessageBox.Show(msg, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void TriggerShowOrHide()
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                this.WindowState = FormWindowState.Minimized;
                this.Hide();
            }
            else if (this.WindowState == FormWindowState.Minimized)
            {
                this.Show();
                this.WindowState = FormWindowState.Normal;
                this.Activate();
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            TriggerShowOrHide();
        }

        protected override void OnClosed(EventArgs e)
        {
            _autoLoginService?.Dispose();
            base.OnClosed(e);
        }
    }

    static class Extensions
    {
        public static bool IsNullOrWhiteSpace(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }
    }
}
