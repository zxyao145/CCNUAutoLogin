using System;
using System.Threading;

namespace CCNUAutoLogin.Core
{
    public class AutoLoginService:IDisposable
    {
        public AutoLoginService(AppConfig config)
        {
            _config = config;
        }
        private Timer _timer;
        private AppConfig _config;
        private ISchoolNetLoginService _loginService;

        public void Start()
        {
            var timeInterval = 10 * 60 * 1000;//每10分钟
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

        public void Stop()
        {
            _timer?.Change(0, -1);
            _timer?.Dispose();
            _loginService = null;
        }

        public void UpdateConfig(AppConfig config)
        {
            _loginService?.Logout();
            _config = config;
            AutoLogin();
        }

        private void AutoLogin()
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
        }

        public void Dispose()
        {
            Stop();
        }
    }
}
