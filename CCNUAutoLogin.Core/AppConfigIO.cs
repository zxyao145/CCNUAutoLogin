using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Jil;

namespace CCNUAutoLogin.Core
{
    public class AppConfigIO
    {
        private static string GetDefaultConfigPath()
        {
            return Path.Combine(AppContext.BaseDirectory, "config.json");
        }

        public static void Write(AppConfig config, string configPath = null)
        {
            configPath ??= GetDefaultConfigPath();
            using StreamWriter sw = new StreamWriter(configPath);
            sw.Write(JSON.Serialize(config, new Options(true)));
        }

        public static AppConfig Read(string configPath = null)
        {
            configPath ??= GetDefaultConfigPath();
            if (File.Exists(configPath))
            {
                string jsonInfo;
                using (StreamReader sr = new StreamReader(configPath))
                {
                    jsonInfo = sr.ReadToEnd();
                }

                AppConfig config = JSON.Deserialize<AppConfig>(jsonInfo);
                return config;
            }

            return null;
        }

    }
}
