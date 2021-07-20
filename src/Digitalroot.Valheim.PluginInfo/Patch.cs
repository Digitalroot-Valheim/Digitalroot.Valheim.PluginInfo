using Digitalroot.Valheim.Common;
using HarmonyLib;
using JetBrains.Annotations;
using System;
using System.Reflection;

namespace Digitalroot.Valheim.PluginInfo
{
  [UsedImplicitly]
  public class Patch
  {
    [HarmonyPatch(typeof(FejdStartup), nameof(FejdStartup.Start))]
    public class PatchFejdStartupStart
    {
      [UsedImplicitly]
      [HarmonyPostfix]
      [HarmonyPriority(Priority.Low)]
      public static void Postfix()
      {
        try
        {
          Log.Trace($"{Main.Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}");

          Main.Instance.OnFejdStartupStart();
        }
        catch (Exception e)
        {
          Log.Error(e);
        }
      }
    }
  }
}
