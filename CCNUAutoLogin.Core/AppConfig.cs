namespace CCNUAutoLogin.Core
{
    public class AppConfig
    {
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