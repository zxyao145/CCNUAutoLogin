﻿using System;
using System.IO;

namespace CCNUAutoLogin.Core
{
    /// <summary>
    /// 日志记录
    /// </summary>
    public static class LogHelper
    {
        private static readonly string LogsDir;
        static LogHelper()
        {
            //var appDir = Path.GetTempPath();
            var appDir = Utils.RealStartupDir; // AppContext.BaseDirectory
            LogsDir = Path.Combine(appDir, "./CCNUAutoLoginLogs");
            Directory.CreateDirectory(LogsDir);
        }

        public static void WriteError(string msg)
        {
            string logPath = GetLogPath();
            using (StreamWriter sw = new StreamWriter(logPath, true))
            {
                sw.WriteLine($"{DateTime.Now.ToShortTimeString()}");
                sw.Write($"Error:\t");
                sw.WriteLine(msg);
                sw.Close();
            }
        }

        public static void WriteInfo(string msg)
        {
            string logPath = GetLogPath();
            using (StreamWriter sw = new StreamWriter(logPath, true))
            {
                sw.WriteLine($"{DateTime.Now.ToShortTimeString()}");
                sw.Write($"Info:\t");
                sw.WriteLine(msg);
                sw.Close();
            }
        }

        private static string GetLogPath()
        {
            var logName = DateTime.Now.ToString("yyyyMMdd")+".log";
            string logPath = Path.Combine(LogsDir, logName);
            return logPath;
        }

    }
}
