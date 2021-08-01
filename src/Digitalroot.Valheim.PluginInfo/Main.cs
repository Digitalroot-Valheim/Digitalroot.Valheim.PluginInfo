using BepInEx;
using Digitalroot.Valheim.Common;
using HarmonyLib;
using JetBrains.Annotations;
using System.Collections.Generic;
using System.Reflection;

namespace Digitalroot.Valheim.PluginInfo
{
  [BepInPlugin(Guid, Name, Version)]
  public class Main : BaseUnityPlugin, ITraceableLogging
  {
    private Harmony _harmony;
    public const string Version = "1.1.0";
    public const string Name = "Digitalroot Plug-in Info";
    // ReSharper disable once MemberCanBePrivate.Global
    public const string Guid = "digitalroot.mods.plugininfo";
    public const string Namespace = "Digitalroot.Valheim." + nameof(PluginInfo);
    public static Main Instance;

    #region Implementation of ITraceableLogging

    /// <inheritdoc />
    public string Source => Namespace;

    #endregion

    public Main()
    {
#if DEBUG
      Log.RegisterSource(this, true);
#endif

      Log.Trace(this, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
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
      Log.Trace(this, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      _harmony?.UnpatchSelf();
    }

    public void OnFejdStartupStart()
    {
      Log.Debug(this, "******* [Digitalroot Plug-ins Loaded] *******");
      foreach (KeyValuePair<string, BepInEx.PluginInfo> pluginInfo in BepInEx.Bootstrap.Chainloader.PluginInfos)
      {
        Log.Debug(this, $"Key: {pluginInfo.Key}");
        Log.Debug(this, $"Value: {pluginInfo.Value}");
        Log.Debug(this, $"GUID: {pluginInfo.Value.Metadata.GUID}");
        Log.Debug(this, $"Name: {pluginInfo.Value.Metadata.Name}");
        Log.Debug(this, $"Version: {pluginInfo.Value.Metadata.Version}");
        Log.Debug(this, $"Location: {pluginInfo.Value.Location}");

        Log.Debug(this, $"Dependencies:");
        foreach (BepInDependency dependency in pluginInfo.Value.Dependencies)
        {
          Log.Debug(this, $"DependencyGUID: {dependency.DependencyGUID}");
          Log.Debug(this, $"Flags: {dependency.Flags}");
          Log.Debug(this, $"MinimumVersion: {dependency.MinimumVersion}");
        }

        Log.Debug(this, $"Incompatibilities:");
        foreach (BepInIncompatibility incompatibility in pluginInfo.Value.Incompatibilities)
        {
          Log.Debug(this, $"DependencyGUID: {incompatibility.IncompatibilityGUID}");
        }

        Log.Debug(this, $"Instance: {pluginInfo.Value.Instance}");
        Log.Debug(this, $"***************************************");
      }

      Log.Debug(this, $"DependencyErrors");
      foreach (string dependencyError in BepInEx.Bootstrap.Chainloader.DependencyErrors)
      {
        Log.Debug(this, $"{dependencyError}");
      }

      Log.Debug(this, $"***************************************");
    }
  }
}
