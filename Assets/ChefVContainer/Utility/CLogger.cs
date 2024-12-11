using System.Diagnostics;
using gs.chef.vcontainer.core.model;

namespace gs.chef.vcontainer.utility
{
    public static class CLogger
    {
        //private static LogState _configLogState;

        static CLogger()
        {
            //_configLogState = ChefSettings.Instance is null ? LogState.None : ChefSettings.Instance.LogState;
            ReportLogLevel();
        }

        private static void ReportLogLevel()
        {
            Log(LogState.Development, $"Chef Logger is READY");
        }

        [Conditional("CHEF_DEBUG")]
        public static void Log(LogState logState, object message)
        {
            /*if (_configLogState < 0)
                return;*/
//#if CHEF_DEBUG
            switch (logState)
            {
                case LogState.Info:
                    UnityEngine.Debug.Log($"[CLog.{logState}] : {message}");
                    break;
                case LogState.Error:
                case LogState.Fatal:
                    UnityEngine.Debug.LogError($"[CLog.{logState}] : {message}");
                    break;
                case LogState.Development:
                case LogState.Warning:
                case LogState.Process:
                case LogState.Analytics:
                    UnityEngine.Debug.LogWarning($"[CLog.{logState}] : {message}");
                    break;
                case LogState.Booster:
                case LogState.Game:
                    UnityEngine.Debug.Log($"<color=magenta>[CLog.{logState}] : {message}</color>");
                    break;
                case LogState.Timer:
                    UnityEngine.Debug.Log($"<color=yellow>[CLog.{logState}] : {message}</color>");
                    break;
                case LogState.Pause:
                    UnityEngine.Debug.Log($"<color=green>[CLog.{logState}] : {message}</color>");
                    UnityEngine.Debug.Break();
                    break;
            }
//#endif
        }
    }
}