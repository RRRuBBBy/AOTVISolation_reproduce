using log4net;
using System;

namespace AOTVI.Common
{
    public class LogService
    {
        private static readonly ILog log = LogManager.GetLogger("AOTVI");

        // 事件
        public static event Action<string> OnLog;

        public static void Info(string msg)
        {
            log.Info(msg);
            OnLog?.Invoke($"[INFO] {DateTime.Now:HH:mm:ss} {msg}");
        }

        public static void Error(string msg, Exception ex)
        {
            log.Error(msg, ex);
            OnLog?.Invoke($"[ERROR] {DateTime.Now:HH:mm:ss} {msg}");
        }
    }
}