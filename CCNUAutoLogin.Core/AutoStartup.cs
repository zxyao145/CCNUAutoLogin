using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Security;
using System.Text;

namespace CCNUAutoLogin.Core
{

    /// <summary>
    /// 开机自启
    /// </summary>
    public class AutoStartup
    {
        static string Key = "CCNUAutoLogin_" + AppContext.BaseDirectory.GetHashCode();
        static string RegistryRunPath = (IntPtr.Size == 4 
            ? @"Software\Microsoft\Windows\CurrentVersion\Run" 
            : @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Run");

        public static void SetStartupPath(string startupPath)
        {
            Key = "CCNUAutoLogin_" + startupPath.GetHashCode();
        }

        public static bool Set(bool enabled)
        {
            RegistryKey runKey = null;
            try
            {
                string path = AppContext.BaseDirectory;
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
                Utils.RunAsAdmin();
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
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
                        Console.WriteLine(e);
                    }
                }
            }
        }

        public static bool Switch()
        {
            bool enabled = !Check();
            RegistryKey runKey = null;
            try
            {
                string path = AppContext.BaseDirectory;
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
                        Console.WriteLine(e);
                    }
                }
            }
        }

        public static bool Check()
        {
            RegistryKey runKey = null;
            try
            {
                string path = AppContext.BaseDirectory;

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
                        Console.WriteLine(e);
                    }
                }
            }
        }
    }
}
