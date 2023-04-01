using Digitalroot.Valheim.Common;
using HarmonyLib;
using System;
using System.Reflection;

namespace Digitalroot.Valheim.PluginInfo
{
  // ReSharper disable once ClassNeverInstantiated.Global
  public class Patch
  {
    [HarmonyPatch(typeof(FejdStartup), nameof(FejdStartup.Start))]
    public class PatchFejdStartupStart
    {
      [HarmonyPostfix]
      [HarmonyPriority(Priority.Low)]
      public static void Postfix()
      {
        try
        {
          Log.Trace(Main.Instance, $"{Main.Namespace}.{MethodBase.GetCurrentMethod()?.DeclaringType?.Name}.{MethodBase.GetCurrentMethod()?.Name}");

          Main.Instance.OnFejdStartupStart();
        }
        catch (Exception e)
        {
          Log.Error(Main.Instance, e);
        }
      }
    }
  }
}
