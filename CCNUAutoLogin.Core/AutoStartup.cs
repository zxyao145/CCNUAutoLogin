using Microsoft.Win32;
using System;
using System.Security;

namespace CCNUAutoLogin.Core
{

    /// <summary>
    /// 开机自启 copy from SSR Project
    /// </summary>
    public class AutoStartup
    {
        /// <summary>
        /// 注册表 key
        /// </summary>
        static string Key = "CCNUAutoLogin";
        /// <summary>
        /// 注册表 路径
        /// </summary>
        static string RegistryRunPath = (IntPtr.Size == 4
            ? @"Software\Microsoft\Windows\CurrentVersion\Run"
            : @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Run");

        /// <summary>
        /// 设置 是否 开机启动
        /// </summary>
        /// <param name="enabled"></param>
        /// <returns></returns>
        public static bool Set(bool enabled)
        {
            RegistryKey runKey = null;
            try
            {
                string path = Utils.ExecutablePath; // AppContext.BaseDirectory
                runKey = Registry.LocalMachine.OpenSubKey(RegistryRunPath, true);
                if (runKey != null)
                {
                    if (enabled)
                    {
                        runKey.SetValue(Key, path);
                    }
                    else
                    {
                        runKey.DeleteValue(Key);
                    }
                    runKey.Close();
                    return true;
                }
                return false;
            }
            catch (SecurityException e)
            {
                return false;
            }
            catch (Exception e)
            {
                LogHelper.WriteError($"AutoStartup: {e.Message}");
                return false;
            }
            finally
            {
                if (runKey != null)
                {
                    try
                    {
                        runKey.Close();
                    }
                    catch (Exception e)
                    {
                        LogHelper.WriteError($"AutoStartup: {e.Message}");
                    }
                }
            }
        }

        /// <summary>
        /// 开机启动切换
        /// </summary>
        /// <returns></returns>
        public static bool Switch()
        {
            bool enabled = !Check();
            RegistryKey runKey = null;
            try
            {
                string path = Utils.ExecutablePath; // AppContext.BaseDirectory
                runKey = Registry.LocalMachine.OpenSubKey(RegistryRunPath, true);
                if (runKey != null)
                {
                    if (enabled)
                    {
                        runKey.SetValue(Key, path);
                    }
                    else
                    {
                        runKey.DeleteValue(Key);
                    }
                    runKey.Close();
                    return true;
                }

                return false;
            }
            catch (Exception e)
            {
                LogHelper.WriteError($"AutoStartup: {e.Message}");
                return false;
            }
            finally
            {
                if (runKey != null)
                {
                    try
                    {
                        runKey.Close();
                    }
                    catch (Exception e)
                    {
                        LogHelper.WriteError($"AutoStartup: {e.Message}");
                    }
                }
            }
        }

        /// <summary>
        /// 监测是否已经允许开机启动
        /// </summary>
        /// <returns></returns>
        public static bool Check()
        {
            RegistryKey runKey = null;
            try
            {
                runKey = Registry.LocalMachine.OpenSubKey(RegistryRunPath, false);
                string[] runList = runKey.GetValueNames();
                runKey.Close();
                foreach (string item in runList)
                {
                    if (item.Equals(Key))
                        return true;
                }
                return false;
            }
            catch (Exception e)
            {
                LogHelper.WriteError($"AutoStartup: {e.Message}");
                return false;
            }
            finally
            {
                if (runKey != null)
                {
                    try
                    {
                        runKey.Close();
                    }
                    catch (Exception e)
                    {
                        LogHelper.WriteError($"AutoStartup: {e.Message}");
                    }
                }
            }
        }
    }
}
