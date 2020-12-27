using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Jil;

namespace CCNUAutoLogin.Core
{
    public class AppConfigIO
    {
        private static string GetDefaultConfigPath(string configFileName = "config.json")
        {
            return Path.Combine(Utils.RealStartupDir, configFileName);
        }

        /// <summary>
        /// login config
        /// </summary>
        /// <param name="config"></param>
        /// <param name="configPath"></param>
        /// <returns></returns>
        public static string Write(LoginConfig config, string configPath = null)
        {
            return Write<LoginConfig>(config, "config.json");
        }

        /// <summary>
        /// login config
        /// </summary>
        /// <param name="configPath"></param>
        /// <returns></returns>
        public static LoginConfig Read(string configPath = null)
        {
            return Read<LoginConfig>("config.json");
        }


        public static T Read<T>(string configFileName)
        {
            var configPath = GetDefaultConfigPath(configFileName);
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

        public static string Write<T>(T config, string configFileName)
        {
            var configPath = GetDefaultConfigPath(configFileName);
            using StreamWriter sw = new StreamWriter(configPath);
            sw.Write(JSON.Serialize<T>(config, new Options(true)));
            return configPath;
        }
    }
}
