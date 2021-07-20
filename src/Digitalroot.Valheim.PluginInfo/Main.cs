using BepInEx;
using Digitalroot.Valheim.Common;
using HarmonyLib;
using JetBrains.Annotations;
using System.Collections.Generic;
using System.Reflection;

namespace Digitalroot.Valheim.PluginInfo
{
  [BepInPlugin(Guid, Name, Version)]
  public class Main : BaseUnityPlugin
  {
    public const string Version = "1.0.2";
    public const string Name = "Digitalroot Plug-in Info";
    private const string Guid = "digitalroot.mods.plugininfo";
    public const string Namespace = nameof(PluginInfo);
    private Harmony _harmony;
    public static Main Instance;

    public Main()
    {
#if DEBUG
      Log.SetSource("PluginInfo");
      Log.EnableTrace();
#endif

      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      Instance = this;
    }

    [UsedImplicitly]
    private void Awake()
    {
      _harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), Guid);
    }

    [UsedImplicitly]
    private void OnDestroy()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      _harmony?.UnpatchSelf();
    }

    public void OnFejdStartupStart()
    {
      Log.Debug("******* [Digitalroot Plug-ins Loaded] *******");
      foreach (KeyValuePair<string, BepInEx.PluginInfo> pluginInfo in BepInEx.Bootstrap.Chainloader.PluginInfos)
      {
        Log.Debug($"Key: {pluginInfo.Key}");
        Log.Debug($"Value: {pluginInfo.Value}");
        Log.Debug($"GUID: {pluginInfo.Value.Metadata.GUID}");
        Log.Debug($"Name: {pluginInfo.Value.Metadata.Name}");
        Log.Debug($"Version: {pluginInfo.Value.Metadata.Version}");
        Log.Debug($"Location: {pluginInfo.Value.Location}");

        Log.Debug($"Dependencies:");
        foreach (BepInDependency dependency in pluginInfo.Value.Dependencies)
        {
          Log.Debug($"DependencyGUID: {dependency.DependencyGUID}");
          Log.Debug($"Flags: {dependency.Flags}");
          Log.Debug($"MinimumVersion: {dependency.MinimumVersion}");
        }

        Log.Debug($"Incompatibilities:");
        foreach (BepInIncompatibility incompatibility in pluginInfo.Value.Incompatibilities)
        {
          Log.Debug($"DependencyGUID: {incompatibility.IncompatibilityGUID}");
        }

        Log.Debug($"Instance: {pluginInfo.Value.Instance}");
        Log.Debug($"***************************************");
      }

      Log.Debug($"DependencyErrors");
      foreach (string dependencyError in BepInEx.Bootstrap.Chainloader.DependencyErrors)
      {
        Log.Debug($"{dependencyError}");
      }

      Log.Debug($"***************************************");
    }
  }
}
