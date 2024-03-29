﻿namespace CCNUAutoLogin.Core
{
    /// <summary>
    /// 登陆信息配置
    /// </summary>
    public class LoginConfig
    {
        /// <summary>
        /// 网络在线监测时间，默认5分钟：5 *60 * 1000 = 300000;
        /// </summary>
        public int OnlineMonitorInterval { get; set; } = 300000;

        /// <summary>
        /// 学号
        /// </summary>
        public string SchNum { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 校园网类型
        /// </summary>
        public string SchNetType { get; set; } = "联通";

        /// <summary>
        /// 连接方式
        /// </summary>
        public string ConnectType { get; set; } = "无线";
    }
}