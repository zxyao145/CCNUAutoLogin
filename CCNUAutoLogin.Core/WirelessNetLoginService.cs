using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Text;

namespace CCNUAutoLogin.Core
{
    /// <summary>
    /// 无线网络登录
    /// </summary>
    public class WirelessNetLoginService : SchoolNetLoginServiceBase
    {
        public override bool Login(LoginConfig config)
        {
            var userName = BuildUserName(config);
            NameValueCollection values = new NameValueCollection
            {
                {"user", userName},
                {"password", config.Password}
            };

            var url = "http://securelogin.arubanetworks.com/auth/index.html/u";
            using WebClient client = new WebClient()
            {
                Headers = new WebHeaderCollection
                {
                    {
                        "Host", "securelogin.arubanetworks.com"
                    },
                    {
                        "User-Agent",
                        "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.110 Safari/537.36"
                    },
                    {"Referer", "http://login.ccnu.edu.cn/"},
                    {
                        "Origin", "http://login.ccnu.edu.cn"
                    }
                }
            };

            client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            try
            {
                byte[] result = client.UploadValues(url, "POST", values);
                if (LoginCheck(result, out var html))
                {
                    LogHelper.WriteInfo($"WirelessNetLogin 登陆成功");
                    return true;
                }
                LogHelper.WriteInfo($"WirelessNetLogin 登陆失败，原因未知。返回内容：{Environment.NewLine} {html}");
                return false;
            }
            catch (Exception e)
            {
                LogHelper.WriteError($"WirelessNetLogin 登陆失败: {e.Message}");
                return false;
            }
        }

        public override bool Logout()
        {
            var logoutUrl = "http://securelogin.arubanetworks.com/cgi-bin/login?cmd=logout";
            using var client = new WebClient();
            try
            {
                client.DownloadData(logoutUrl);
                return true;
            }
            catch (Exception e)
            {
                LogHelper.WriteError($"WirelessNetLogin 退出登陆失败：{e.Message}");
                return false;
            }
        }
    }
}
