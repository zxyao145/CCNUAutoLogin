using System;
using System.Threading;

namespace CCNUAutoLogin.Core
{
    /// <summary>
    /// 自动登陆服务，每10分钟检测一次是否断网
    /// </summary>
    public class AutoLoginService:IDisposable
    {
        public AutoLoginService(LoginConfig config)
        {
            _config = config;
        }
        private Timer _timer;
        private LoginConfig _config;
        private ISchoolNetLoginService _loginService;

        public void Start()
        {
            var timeInterval = _config.OnlineMonitorInterval;//每10分钟
            try
            {
                _timer = new Timer(state =>
                {
                    var isOnline = SchoolNetLoginServiceBase.IsOnline();
                    if (!isOnline)
                    {
                        AutoLogin();
                    }
                }, null, 0, timeInterval);
            }
            catch (Exception e)
            {
                LogHelper.WriteError($"Timer 出错，{e.Message}");
            }
        }

        /// <summary>
        /// 停止监测
        /// </summary>
        public void Stop()
        {
            _timer?.Change(0, -1);
            _timer?.Dispose();
            _loginService = null;
        }

        /// <summary>
        /// 更新配置
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public bool UpdateConfig(LoginConfig config)
        {
            _loginService?.Logout();
            if (config.OnlineMonitorInterval != _config.OnlineMonitorInterval)
            {
                _timer.Change(0, config.OnlineMonitorInterval);
            }
            _config = config;
            return AutoLogin();
        }

        /// <summary>
        /// 5次自动登陆尝试
        /// </summary>
        /// <returns></returns>
        private bool AutoLogin()
        {
            var retryTimes = 5;
            bool isSuccess = false;
            _loginService = SchoolNetLoginServiceFactory.Create(_config);

            if (_loginService != null)
            {
                while (retryTimes > 0)
                {
                    isSuccess = _loginService.Login(_config);
                    if (isSuccess)
                    {
                        break;
                    }
                    else
                    {
                        var isOnline = SchoolNetLoginServiceBase.IsOnline();
                        if (isOnline)
                        {
                            isSuccess = true;
                            break;
                        }
                    }

                    retryTimes--;
                    Thread.Sleep((5 - retryTimes) * 2000);
                }
                if (!isSuccess)
                {
                    LogHelper.WriteError($"尝试自动登陆5次后失败");
                }
            }

            if (isSuccess) return true;
            return SchoolNetLoginServiceBase.IsOnline();
        }

        /// <summary>
        /// 手动登陆
        /// </summary>
        /// <returns></returns>
        public bool LoginManual()
        {
            return SchoolNetLoginServiceBase.IsOnline() || AutoLogin();
        }

        public void Dispose()
        {
            Stop();
        }
    }
}
