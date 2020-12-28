using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;

namespace CCNUAutoLogin.Core
{
    public interface ISchoolNetLoginService
    {
        /// <summary>
        /// 校园网 登陆
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public bool Login(LoginConfig config);

        /// <summary>
        /// 校园网 退出登陆
        /// </summary>
        /// <returns></returns>
        public bool Logout();
    }

    public abstract class SchoolNetLoginServiceBase : ISchoolNetLoginService
    {
        /// <summary>
        /// 网络是否在线
        /// </summary>
        /// <returns></returns>
        internal static bool IsOnline()
        {
            return PingBaidu();
        }

        /// <summary>
        /// ping www.baidu.com
        /// </summary>
        /// <returns></returns>
        private static bool PingBaidu()
        {
            try
            {
                return PingIpOrDomainName("www.baidu.com");
            }
            catch (Exception)
            {
                return false;
            }

        }

        /// <summary>
        /// ping 百度，判断网络是否联通
        /// </summary>
        /// <param name="strIpOrDName"></param>
        /// <returns></returns>
        private static bool PingIpOrDomainName(string strIpOrDName)
        {
            try
            {
                Ping objPingSender = new Ping();
                PingOptions objPinOptions = new PingOptions();
                objPinOptions.DontFragment = true;
                string data = "";
                byte[] buffer = Encoding.UTF8.GetBytes(data);
                int intTimeout = 120;
                PingReply objPinReply = objPingSender.Send(strIpOrDName, intTimeout, buffer, objPinOptions);
                if (objPinReply != null)
                {
                    string strInfo = objPinReply.Status.ToString();
                    if (strInfo == "Success")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 根据不同运营商构建用户名
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        protected string BuildUserName(LoginConfig config)
        {
            if (config.SchNetType.Contains("电信"))
            {
                return config.SchNum + "@chinanet";
            }
            else if (config.SchNetType.Contains("移动"))
            {
                return config.SchNum + "@cmcc";
            }
            else if (config.SchNetType.Contains("联通"))
            {
                return config.SchNum + "@unicom";
            }
            return config.SchNum;
        }

        public abstract bool Login(LoginConfig config);

        public abstract bool Logout();

        /// <summary>
        /// 登陆成功监测
        /// </summary>
        /// <param name="data"></param>
        /// <param name="html"></param>
        /// <returns></returns>
        protected bool LoginCheck(byte[] data, out string html)
        {
            html = data != null
                ? Encoding.GetEncoding("GB2312").GetString(data)
                : "";
            return (html.Contains("External Welcome Page") 
                    || html.Contains("登录成功") 
                    || PingBaidu());
        }
    }

    public class SchoolNetLoginServiceFactory
    {
        /// <summary>
        /// 根据网络连接类型创建一个 ISchoolNetLoginService
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static ISchoolNetLoginService Create(LoginConfig config)
        {
            if (config.ConnectType == "有线")
            {
                return new WiredNetLoginService();
            }
            else if (config.ConnectType == "无线")
            {
                return new WirelessNetLoginService();
            }

            return null;
        }
    }
}
