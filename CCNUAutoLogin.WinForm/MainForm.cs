﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CCNUAutoLogin.Core;
using Jil;

namespace CCNUAutoLogin.WinForm
{
    public partial class MainForm : Form
    {
        private AutoLoginService _autoLoginService;
        private bool _isAutoStartup = false;
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

        public MainForm()
        {
            InitializeComponent();
            this.ShowInTaskbar = true;
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            CheckIsAutoStartup();
            var config = FillUiByConfig();
            if (config == null)
            {
                ShowForm();
            }
            else
            {
                HideForm();
                _autoLoginService = new AutoLoginService(config);
                _autoLoginService.Start();
            }

            #region appNotifyIcon 邮件菜单事件绑定

            configApp.Click += ConfigApp_Click;
            exitApp.Click += ExitApp_Click;
            loginManual.Click += LoginManual_Click;
            autoStartup.Click += AutoStartup_Click;
            openConfigDir.Click += OpenConfigDir_Click; 

            #endregion
        }
        
        /// <summary>
        /// 根据配置文件检查是否已经设置了开机启动
        /// </summary>
        private void CheckIsAutoStartup()
        {
            var appConfigFile = Path.Combine(Utils.RealStartupDir, "app.config");
            if (File.Exists(appConfigFile))
            {
                var appConfig = AppConfigIO.Read<AppConfig>("app.config");
                _isAutoStartup = appConfig.AutoStartup;
            }
            else
            {
                var config = new AppConfig
                {
                    AutoStartup = false
                };
                var savePath = AppConfigIO.Write<AppConfig>(config, "app.config");
            }

            SetAutoStartupMenuItemText();
        }

        /// <summary>
        /// 修改开机启动的显示文字
        /// </summary>
        private void SetAutoStartupMenuItemText()
        {
            autoStartup.Text = _isAutoStartup ? "[√]开机启动" : "[x]开机启动";
        }

        /// <summary>
        /// 根据设置填充UI
        /// </summary>
        /// <returns></returns>
        private LoginConfig FillUiByConfig()
        {
            var config = AppConfigIO.Read();
            if (config != null)
            {
                schoolNum.Text = config.SchNum;
                password.Text = config.Password;
                onlineMonitorInterval.Text = config.OnlineMonitorInterval.ToString();

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
                        if (rBtn.Text == config.ConnectType)
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

        /// <summary>
        /// 开启启动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AutoStartup_Click(object sender, EventArgs e)
        {
            _isAutoStartup = !_isAutoStartup;
            SetAutoStartupMenuItemText();

            if (!AutoStartup.Set(_isAutoStartup))
            {
                var dialogResult = MessageBox.Show("设置或取消开机启动需要管理员的权限，是否以管理员权限运行？", "提示", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    Utils.RunAsAdmin(_isAutoStartup ? "true" : "false");
                    Dispose();
                    Close();
                }
            }
        }

        /// <summary>
        /// 打开配置文件目录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenConfigDir_Click(object sender, EventArgs e)
        {
            var file = Path.Combine(Utils.RealStartupDir, "config.json");
            if (File.Exists(file))
            {
                Process.Start("Explorer", "/select," + file);
            }
            else
            {
                Process.Start("Explorer", Utils.RealStartupDir);
            }
        }

        /// <summary>
        /// 手动登陆
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoginManual_Click(object sender, EventArgs e)
        {
            if (_autoLoginService != null)
            {
                var isSuccess = _autoLoginService.LoginManual();
                var msg = isSuccess ? "手动登陆成功！" : "手动登陆失败！";
                MessageBox.Show(msg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("请先配置登陆信息！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
        }

        /// <summary>
        /// 退出程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// 打开配置界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConfigApp_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                ShowForm();
            }
        }

        #endregion

        #region 主窗口事件

        /// <summary>
        /// Notify被点击时候切换配置窗体显示与隐藏的状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void appNotifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)//判断鼠标的按键
            {
                // 点击时判断form是否显示,显示就隐藏,隐藏就显示
                TriggerShowOrHide();
            }
        }

        /// <summary>
        /// 网络类型切换时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNetTypeChanged(object sender, EventArgs e)
        {
            if (sender is RadioButton rtb)
            {
                _schNetType = rtb.Text;
            }
        }

        /// <summary>
        /// 网络连接类型切换时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLanOrWlanTypeChanged(object sender, EventArgs e)
        {
            if (sender is RadioButton rtb)
            {
                _connectType = rtb.Text;
            }
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnSave_Click(object sender, EventArgs e)
        {
            btnSave.Enabled = false;
            HideForm();
            await Task.Run(() =>
            {
                var config = new LoginConfig()
                {
                    SchNum = schoolNum.Text,
                    Password = password.Text,
                    ConnectType = _connectType,
                    SchNetType = _schNetType
                };
                if (Verify(config, onlineMonitorInterval.Text))
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

                    MessageBox.Show($"信息保存成功！{Environment.NewLine} {configPath}", "提示", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    if (!_isAutoStartup)
                    {
                        if (AutoStartup.Set(true))
                        {
                            // TriggerShowOrHide();
                            _isAutoStartup = true;
                            AppConfigIO.Write<AppConfig>(new AppConfig
                            {
                                AutoStartup = _isAutoStartup
                            }, "app.config");
                            HideForm();
                        }
                        else
                        {
                            var dialogResult = MessageBox.Show("开机自启动需要管理员的权限，是否以管理员权限运行？", "提示",
                                MessageBoxButtons.YesNo,
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
            });
            btnSave.Enabled = true;
        }

        #endregion

        /// <summary>
        /// 验证config是否有效
        /// </summary>
        /// <param name="config"></param>
        /// <param name="onlineMonitorIntervalText"></param>
        /// <returns>true：有效；false：无效</returns>
        private bool Verify(LoginConfig config, string onlineMonitorIntervalText)
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

            if (int.TryParse(onlineMonitorIntervalText, out var onlineMonitorIntervalVal))
            {
                if (onlineMonitorIntervalVal < 1000 * 10)
                {
                    messages.Add("监测时间间隔最小为 10000毫秒（10秒）");
                }
                else
                {
                    config.OnlineMonitorInterval = onlineMonitorIntervalVal;
                }
            }
            else
            {
                messages.Add("监测时间只能为数字");
            }


            if (messages.Count > 0)
            {
                var msg = string.Join(Environment.NewLine, messages);
                MessageBox.Show(this, msg, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        #region 窗体状态切换

        /// <summary>
        /// 切换显示或隐藏状态
        /// </summary>
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

        #endregion

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

        private void onlineMonitorInterval_KeyPress(object sender, KeyPressEventArgs e)
        {
            //判断按键是不是数字和删除（backspace）
            if ((e.KeyChar < 48 || e.KeyChar > 57) && e.KeyChar != 8)
            {
                e.Handled = true;
            }
            else
            {
                // 禁止首字母为0
                var txt = onlineMonitorInterval.Text;
                if ((txt.Length == 0) || (onlineMonitorInterval.SelectionLength == txt.Length))
                {
                    if (e.KeyChar == 48)
                    {
                        e.Handled = true;

                    }
                }
            }
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
