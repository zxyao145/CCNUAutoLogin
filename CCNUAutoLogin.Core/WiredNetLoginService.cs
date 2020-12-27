using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace CCNUAutoLogin.Core
{
    /// <summary>
    /// 有线网络登录
    /// </summary>
    public class WiredNetLoginService : SchoolNetLoginServiceBase
    {
        public override bool Login(LoginConfig config)
        {
            var userName = BuildUserName(config);
            var suffix = "0";
            if (config.SchNetType == "电信")
            {
                suffix = "1";
            }
            else if (config.SchNetType == "移动")
            {
                suffix = "2";
            }
            else if (config.SchNetType == "联通")
            {
                suffix = "3";
            }

            string postString = $"DDDDD={userName}&upass={config.Password}&suffix={suffix}&0MKKey=123";
            byte[] postData = Encoding.UTF8.GetBytes(postString);
            var downloadUrl = "http://l.ccnu.edu.cn/0.htm"; // "http://10.220.250.50/0.htm";

            using WebClient webClient = new WebClient
            {
                Headers = new WebHeaderCollection
                {
                    {"Host", "10.220.250.50"},
                    {
                        "User-Agent",
                        "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.110 Safari/537.36"
                    },
                    {"Referer", "http://10.220.250.50/0.htm"}
                }
            };

            byte[] responseData = webClient.UploadData(downloadUrl, "POST", postData);
            if (LoginCheck(responseData, out var html))
            {
                LogHelper.WriteInfo($"WiredNetLogin 登陆成功");
                return true;
            }
            LogHelper.WriteInfo($"WiredNetLogin 登陆失败，原因未知。返回内容：{Environment.NewLine} {html}");
            return false;
        }

        public override bool Logout()
        {
            var logoutUrl = "http://l.ccnu.edu.cn/F.htm";
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
