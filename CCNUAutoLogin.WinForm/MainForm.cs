﻿using System;
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
                ShowForm();
            }
            else
            {
                HideForm();
            }

            _autoLoginService?.Start();

            configApp.Click += ConfigApp_Click;
            exitApp.Click += ExitApp_Click;
            loginManual.Click += LoginManual_Click;
        }

        private string _schNetType = "联通";
        private string _connectType = "无线";

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

        #region 右键菜单事件

        private void LoginManual_Click(object sender, EventArgs e)
        {
            if (_autoLoginService != null)
            {
                var isSuccess = _autoLoginService?.LoginManual();
                var msg = isSuccess ? "手动登陆成功！" : "手动登陆失败！";
                MessageBox.Show(this, msg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(this, "请先配置登陆信息！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
        }

        private void ExitApp_Click(object sender, EventArgs e)
        {
            this.Activate();
            //右键退出事件
            if (MessageBox.Show(this, "是否需要关闭程序？", "提示:", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)//出错提示
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
                ShowForm();
            }
        }

        #endregion

        #region 主窗口事件

        private void appNotifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)//判断鼠标的按键
            {
                // 点击时判断form是否显示,显示就隐藏,隐藏就显示
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

                MessageBox.Show(this, $"信息保存成功！{Environment.NewLine} {configPath}", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (AutoStartup.Set(true))
                {
                    // TriggerShowOrHide();
                    HideForm();
                }
                else
                {
                    var dialogResult = MessageBox.Show(this, "开机自启动需要管理员的权限，是否以管理员权限运行？", "提示", MessageBoxButtons.YesNo,
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

        #endregion

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
                MessageBox.Show(this, msg, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void TriggerShowOrHide()
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                HideForm();
            }
            else if (this.WindowState == FormWindowState.Minimized)
            {
                ShowForm();
            }
        }

        /// <summary>
        /// 显示Form
        /// </summary>
        private void ShowForm()
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.Activate();
        }

        /// <summary>
        /// 隐藏Form
        /// </summary>
        private void HideForm()
        {
            this.WindowState = FormWindowState.Minimized;
            this.Hide();
        }

        #region override

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            // TriggerShowOrHide();
            HideForm();
        }

        protected override void OnClosed(EventArgs e)
        {
            _autoLoginService?.Dispose();
            base.OnClosed(e);
        } 

        #endregion
    }

    static class Extensions
    {
        public static bool IsNullOrWhiteSpace(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }
    }
}
