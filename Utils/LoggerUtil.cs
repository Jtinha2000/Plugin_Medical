using Rocket.Core.Logging;
using SDG.Unturned;
using System;

namespace UMedical.Utils
{
  public static class LoggerUtil
  {
    public static int ActualLog { get; set; } = 1;

    public static void SendDebugLog(string Message, params string[] Values)
    {
      if (!Main.Instance.Configuration.Instance.MiscConfig.DebugMode)
        return;
      Logger.Log("[ " + Provider.serverName + " ] " + string.Format(Message, (object[]) Values) + string.Format(" #{0}", (object) LoggerUtil.ActualLog), ConsoleColor.Red);
      ++LoggerUtil.ActualLog;
    }
  }
}
