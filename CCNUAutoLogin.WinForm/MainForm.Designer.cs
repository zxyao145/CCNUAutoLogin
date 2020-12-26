
namespace CCNUAutoLogin.WinForm
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.appNotifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.appNotificationContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.configApp = new System.Windows.Forms.ToolStripMenuItem();
            this.exitApp = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.schoolNum = new System.Windows.Forms.TextBox();
            this.labelPwd = new System.Windows.Forms.Label();
            this.password = new System.Windows.Forms.TextBox();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.netTypePanel = new System.Windows.Forms.Panel();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.connectTypePanel = new System.Windows.Forms.Panel();
            this.radioButton7 = new System.Windows.Forms.RadioButton();
            this.radioButton8 = new System.Windows.Forms.RadioButton();
            this.btnSave = new System.Windows.Forms.Button();
            this.loginManual = new System.Windows.Forms.ToolStripMenuItem();
            this.appNotificationContextMenuStrip.SuspendLayout();
            this.netTypePanel.SuspendLayout();
            this.connectTypePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // appNotifyIcon
            // 
            this.appNotifyIcon.ContextMenuStrip = this.appNotificationContextMenuStrip;
            this.appNotifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("appNotifyIcon.Icon")));
            this.appNotifyIcon.Text = "CCNUAutoLogin";
            this.appNotifyIcon.Visible = true;
            this.appNotifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.appNotifyIcon_MouseDoubleClick);
            // 
            // appNotificationContextMenuStrip
            // 
            this.appNotificationContextMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.appNotificationContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.configApp,
            this.loginManual,
            this.exitApp});
            this.appNotificationContextMenuStrip.Name = "appNotificationContextMenuStrip";
            this.appNotificationContextMenuStrip.Size = new System.Drawing.Size(139, 76);
            this.appNotificationContextMenuStrip.Text = "CCNUAutoLogin";
            // 
            // configApp
            // 
            this.configApp.Name = "configApp";
            this.configApp.Size = new System.Drawing.Size(138, 24);
            this.configApp.Text = "设置";
            // 
            // exitApp
            // 
            this.exitApp.Name = "exitApp";
            this.exitApp.Size = new System.Drawing.Size(138, 24);
            this.exitApp.Text = "退出";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(50, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "学号";
            // 
            // schoolNum
            // 
            this.schoolNum.Location = new System.Drawing.Point(95, 16);
            this.schoolNum.Name = "schoolNum";
            this.schoolNum.Size = new System.Drawing.Size(228, 27);
            this.schoolNum.TabIndex = 1;
            // 
            // labelPwd
            // 
            this.labelPwd.AutoSize = true;
            this.labelPwd.Location = new System.Drawing.Point(50, 52);
            this.labelPwd.Name = "labelPwd";
            this.labelPwd.Size = new System.Drawing.Size(39, 20);
            this.labelPwd.TabIndex = 0;
            this.labelPwd.Text = "密码";
            // 
            // password
            // 
            this.password.Location = new System.Drawing.Point(95, 49);
            this.password.Name = "password";
            this.password.Size = new System.Drawing.Size(228, 27);
            this.password.TabIndex = 2;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(7, 8);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(75, 24);
            this.radioButton1.TabIndex = 3;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "校园网";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.OnNetTypeChanged);
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Checked = true;
            this.radioButton2.Location = new System.Drawing.Point(88, 8);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(60, 24);
            this.radioButton2.TabIndex = 4;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "联通";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // netTypePanel
            // 
            this.netTypePanel.Controls.Add(this.radioButton1);
            this.netTypePanel.Controls.Add(this.radioButton4);
            this.netTypePanel.Controls.Add(this.radioButton3);
            this.netTypePanel.Controls.Add(this.radioButton2);
            this.netTypePanel.Location = new System.Drawing.Point(50, 80);
            this.netTypePanel.Name = "netTypePanel";
            this.netTypePanel.Size = new System.Drawing.Size(282, 38);
            this.netTypePanel.TabIndex = 3;
            // 
            // radioButton4
            // 
            this.radioButton4.AutoSize = true;
            this.radioButton4.Location = new System.Drawing.Point(220, 8);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(60, 24);
            this.radioButton4.TabIndex = 6;
            this.radioButton4.TabStop = true;
            this.radioButton4.Text = "电信";
            this.radioButton4.UseVisualStyleBackColor = true;
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(154, 8);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(60, 24);
            this.radioButton3.TabIndex = 5;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "移动";
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // connectTypePanel
            // 
            this.connectTypePanel.Controls.Add(this.radioButton7);
            this.connectTypePanel.Controls.Add(this.radioButton8);
            this.connectTypePanel.Location = new System.Drawing.Point(50, 124);
            this.connectTypePanel.Name = "connectTypePanel";
            this.connectTypePanel.Size = new System.Drawing.Size(282, 38);
            this.connectTypePanel.TabIndex = 3;
            // 
            // radioButton7
            // 
            this.radioButton7.AutoSize = true;
            this.radioButton7.Checked = true;
            this.radioButton7.Location = new System.Drawing.Point(154, 8);
            this.radioButton7.Name = "radioButton7";
            this.radioButton7.Size = new System.Drawing.Size(60, 24);
            this.radioButton7.TabIndex = 8;
            this.radioButton7.TabStop = true;
            this.radioButton7.Text = "无线";
            this.radioButton7.UseVisualStyleBackColor = true;
            // 
            // radioButton8
            // 
            this.radioButton8.AutoSize = true;
            this.radioButton8.Location = new System.Drawing.Point(88, 8);
            this.radioButton8.Name = "radioButton8";
            this.radioButton8.Size = new System.Drawing.Size(60, 24);
            this.radioButton8.TabIndex = 7;
            this.radioButton8.Text = "有线";
            this.radioButton8.UseVisualStyleBackColor = true;
            this.radioButton8.AppearanceChanged += new System.EventHandler(this.OnLanOrWlanTypeChanged);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(50, 168);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(273, 38);
            this.btnSave.TabIndex = 9;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // loginManual
            // 
            this.loginManual.Name = "loginManual";
            this.loginManual.Size = new System.Drawing.Size(138, 24);
            this.loginManual.Text = "手动登陆";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(374, 227);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.connectTypePanel);
            this.Controls.Add(this.netTypePanel);
            this.Controls.Add(this.password);
            this.Controls.Add(this.labelPwd);
            this.Controls.Add(this.schoolNum);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CCNUAutoLogin";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.appNotificationContextMenuStrip.ResumeLayout(false);
            this.netTypePanel.ResumeLayout(false);
            this.netTypePanel.PerformLayout();
            this.connectTypePanel.ResumeLayout(false);
            this.connectTypePanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NotifyIcon appNotifyIcon;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox schoolNum;
        private System.Windows.Forms.Label labelPwd;
        private System.Windows.Forms.TextBox password;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.Panel netTypePanel;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.RadioButton radioButton4;
        private System.Windows.Forms.Panel connectTypePanel;
        private System.Windows.Forms.RadioButton radioButton7;
        private System.Windows.Forms.RadioButton radioButton8;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.ContextMenuStrip appNotificationContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem configApp;
        private System.Windows.Forms.ToolStripMenuItem exitApp;
        private System.Windows.Forms.ToolStripMenuItem loginManual;
    }
}