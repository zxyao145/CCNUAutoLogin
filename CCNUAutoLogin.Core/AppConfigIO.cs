using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Jil;

namespace CCNUAutoLogin.Core
{
    /// <summary>
    /// 配置文件读写
    /// </summary>
    public class AppConfigIO
    {
        private static string GetDefaultConfigPath(string configFileName = "config.json")
        {
            return Path.Combine(Utils.RealStartupDir, configFileName);
        }

        /// <summary>
        /// 写 登陆信息配置
        /// </summary>
        /// <param name="config"></param>
        /// <param name="configPath"></param>
        /// <returns></returns>
        public static string Write(LoginConfig config, string configPath = null)
        {
            return Write<LoginConfig>(config, "config.json");
        }

        /// <summary>
        /// 读 登陆信息配置
        /// </summary>
        /// <param name="configPath"></param>
        /// <returns></returns>
        public static LoginConfig Read(string configPath = null)
        {
            return Read<LoginConfig>("config.json");
        }

        /// <summary>
        /// 写 配置文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="configFileName"></param>
        /// <returns></returns>
        public static T Read<T>(string configFileName)
        {
            var configPath = GetDefaultConfigPath(configFileName);
            LogHelper.WriteInfo("读取配置文件：" + configPath);
            if (File.Exists(configPath))
            {
                string jsonInfo;
                using (StreamReader sr = new StreamReader(configPath))
                {
                    jsonInfo = sr.ReadToEnd();
                }

                T config = JSON.Deserialize<T>(jsonInfo);
                return config;
            }

            return default(T);
        }

        /// <summary>
        /// 读 配置文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="config"></param>
        /// <param name="configFileName"></param>
        /// <returns></returns>
        public static string Write<T>(T config, string configFileName)
        {
            var configPath = GetDefaultConfigPath(configFileName);
            using StreamWriter sw = new StreamWriter(configPath);
            sw.Write(JSON.Serialize<T>(config, new Options(true)));
            return configPath;
        }
    }
}
