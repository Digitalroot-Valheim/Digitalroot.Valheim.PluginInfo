using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Configuration;
using Digitalroot.Valheim.Common;
using HarmonyLib;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Digitalroot.Valheim.PluginInfo
{
  [BepInPlugin(Guid, Name, Version)]
  [BepInDependency(JVLGuid, BepInDependency.DependencyFlags.SoftDependency)]
  public partial class Main : BaseUnityPlugin, ITraceableLogging
  {
    private Harmony _harmony;
    [UsedImplicitly] public static ConfigEntry<int> NexusId;
    public static Main Instance;
    private const string JVLGuid = "com.jotunn.jotunn";
    private const string CacheName = "chainloader";

    #region Implementation of ITraceableLogging

    /// <inheritdoc />
    public string Source => Namespace;

    /// <inheritdoc />
    public bool EnableTrace { get; }

    #endregion

    public Main()
    {
      try
      {
#if DEBUG
        EnableTrace = true;
#else
        EnableTrace = false;
#endif
        Instance = this;
        NexusId = Config.Bind("General", "NexusID", 1302, new ConfigDescription("Nexus mod ID for updates", null, new ConfigurationManagerAttributes { Browsable = false, ReadOnly = true }));
        Log.RegisterSource(Instance);
        Log.Trace(Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod()?.Name}()");
      }
      catch (Exception e)
      {
        ZLog.LogError(e);
      }
    }

    [UsedImplicitly] 
    private void Awake()
    {
      try
      {
        Log.Trace(Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod()?.Name}()");
        _harmony = Harmony.CreateAndPatchAll(typeof(Main).Assembly, Guid);
      }
      catch (Exception e)
      {
        Log.Error(Instance, e);
      }
    }

    [UsedImplicitly] 
    private void OnDestroy()
    {
      try
      {
        Log.Trace(Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod()?.Name}()");
        _harmony?.UnpatchSelf();
      }
      catch (Exception e)
      {
        Log.Error(Instance, e);
      }
    }

    public void OnFejdStartupStart()
    {
      try
      {
        Log.Trace(Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod()?.Name}()");

        HandleBepInExData();
        HandleDupModDetection();

        if (Common.Utils.DoesPluginExist(JVLGuid))
        {
          HandleJVLData();
        }
      }
      catch (Exception e)
      {
        Log.Error(Instance, e);
      }
    }

    private void HandleBepInExData()
    {
      try
      {
        Log.Debug(Instance, "******* [Digitalroot Plug-ins Loaded] *******");

        foreach (KeyValuePair<string, BepInEx.PluginInfo> pluginInfo in Chainloader.PluginInfos)
        {
          Log.Debug(Instance, $"Key: {pluginInfo.Key}");
          Log.Debug(Instance, $"Value: {pluginInfo.Value}");
          Log.Debug(Instance, $"GUID: {pluginInfo.Value.Metadata.GUID}");
          Log.Debug(Instance, $"Name: {pluginInfo.Value.Metadata.Name}");
          Log.Debug(Instance, $"Version: {pluginInfo.Value.Metadata.Version}");
          Log.Debug(Instance, $"Location: {pluginInfo.Value.Location}");

          Log.Debug(Instance, $"Dependencies:");
          foreach (BepInDependency dependency in pluginInfo.Value.Dependencies)
          {
            Log.Debug(Instance, $"DependencyGUID: {dependency.DependencyGUID}");
            Log.Debug(Instance, $"Flags: {dependency.Flags}");
            Log.Debug(Instance, $"MinimumVersion: {dependency.MinimumVersion}");
          }

          Log.Debug(Instance, $"Incompatibilities:");
          foreach (BepInIncompatibility incompatibility in pluginInfo.Value.Incompatibilities)
          {
            Log.Debug(Instance, $"DependencyGUID: {incompatibility.IncompatibilityGUID}");
          }

          Log.Debug(Instance, $"Instance: {pluginInfo.Value.Instance}");
          Log.Debug(Instance, $"***************************************");
        }

        Log.Debug(Instance, $"DependencyErrors");
        foreach (string dependencyError in Chainloader.DependencyErrors)
        {
          Log.Debug(Instance, $"{dependencyError}");
        }

        Log.Debug(Instance, $"***************************************");
      }
      catch (Exception e)
      {
        Log.Error(Instance, e);
      }
    }

    private void HandleDupModDetection()
    {
      try
      {
        Log.Trace(Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod()?.Name}()");
        Log.Debug(Instance, "******* [Digitalroot Duplicate Mod Info ] *******");

        var plugins = GetPlugins().ToList();

        foreach (var pluginGroup in plugins.GroupBy(info => info.Metadata.GUID)
          .Where(pi => pi.Count() >= 2)
        )
        {
          Log.Warning(Instance, $"Duplicates found for '{pluginGroup.Key}', Count {pluginGroup.Count()}");
          foreach (var pluginInfo in pluginGroup)
          {
            Log.Warning(Instance, $"  Name: {pluginInfo.Metadata.Name}");
            Log.Warning(Instance, $"  GUID: {pluginInfo.Metadata.GUID}");
            Log.Warning(Instance, $"  Version: {pluginInfo.Metadata.Version}");
            Log.Warning(Instance, $"  Location: {pluginInfo.Location}");
            Log.Warning(Instance, $"  ---------------------------------------");
          }
        }
        Log.Debug(Instance, $"***************************************");
      }
      catch (Exception e)
      {
        Log.Error(Instance, e);
      }
    }

    private void HandleJVLData()
    {
      try
      {
        Log.Trace(Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod()?.Name}()");

        Log.Debug(Instance, "******* [Digitalroot JVL Info ] *******");
        foreach (var modInfo in Jotunn.Utils.ModRegistry.GetMods())
        {
          Log.Debug(Instance, $"Name: {modInfo.Name}");
          Log.Debug(Instance, $"Version: {modInfo.Version}");
          Log.Debug(Instance, $"GUID: {modInfo.GUID}");

          if (modInfo.Commands.Any())
          {
            Log.Debug(Instance, string.Empty);
            Log.Debug(Instance, $"Commands:");
            foreach (var consoleCommand in modInfo.Commands)
            {
              Log.Debug(Instance, $"{consoleCommand.Name} - {consoleCommand.Help}");
            }
          }

          if (modInfo.Prefabs.Any())
          {
            Log.Debug(Instance, string.Empty);
            Log.Debug(Instance, $"Prefabs:");
            foreach (var customPrefab in modInfo.Prefabs)
            {
              Log.Debug(Instance, $"{customPrefab.Prefab.name}");
            }
          }

          if (modInfo.Items.Any())
          {
            Log.Debug(Instance, string.Empty);
            Log.Debug(Instance, $"Items: (Token/ItemName => PrefabName)");
            foreach (var customItem in modInfo.Items)
            {
              Log.Debug(Instance, $"{customItem.ItemDrop.m_itemData.m_shared.m_name} => {customItem.ItemPrefab.name}");
            }
          }

          if (modInfo.Recipes.Any())
          {
            Log.Debug(Instance, string.Empty);
            Log.Debug(Instance, $"Recipes: (RecipesName => PrefabName)");
            foreach (var customRecipe in modInfo.Recipes)
            {
              Log.Debug(Instance, $"{customRecipe.Recipe.name} => {customRecipe.Recipe.m_item.name.Replace("JVLmock_", string.Empty)}");
            }
          }

          if (modInfo.ItemConversions.Any())
          {
            Log.Debug(Instance, string.Empty);
            Log.Debug(Instance, $"ItemConversions: (PieceName: PrefabName => PrefabName)");
            foreach (var customItemConversion in modInfo.ItemConversions)
            {
              Log.Debug(Instance, $"{customItemConversion.Config.Station}: {customItemConversion.Config.FromItem} => {customItemConversion.Config.ToItem}");
            }
          }

          if (modInfo.PieceTables.Any())
          {
            Log.Debug(Instance, string.Empty);
            Log.Debug(Instance, $"PieceTables: (PieceTableName: (CategoryNames))");
            foreach (var customPieceTable in modInfo.PieceTables)
            {
              Log.Debug(Instance, $"{customPieceTable.PieceTable}: ({customPieceTable.Categories.Join(delimiter: ", ")})");
            }
          }

          if (modInfo.Pieces.Any())
          {
            Log.Debug(Instance, string.Empty);
            Log.Debug(Instance, $"Pieces: (PieceTableName: Token/PieceName => PrefabName)");
            foreach (var customPiece in modInfo.Pieces)
            {
              var piece = customPiece.PiecePrefab.GetComponent<Piece>();
              Log.Debug(Instance, $"{customPiece.PieceTable}: {piece?.m_name} => {customPiece.Piece.name}");
            }
          }

          if (modInfo.StatusEffects.Any())
          {
            Log.Debug(Instance, string.Empty);
            Log.Debug(Instance, $"StatusEffects: (Token/StatusEffectName)");
            foreach (var customStatusEffect in modInfo.StatusEffects)
            {
              Log.Debug(Instance, $"{customStatusEffect.StatusEffect.name}");
            }
          }

          if (modInfo.Translations.Any())
          {
            Log.Debug(Instance, string.Empty);
            Log.Debug(Instance, $"Translations: (Token => Value)");
            foreach (var customLocalization in modInfo.Translations)
            {
              foreach (var language in customLocalization.GetLanguages())
              {
                Log.Debug(Instance, $"[{language}]");
                foreach (var translation in customLocalization.GetTranslations(language))
                {
                  Log.Debug(Instance, $"${translation.Key} => {translation.Value}");
                }

                Log.Debug(Instance, string.Empty);
              }
            }
          }

          Log.Debug(Instance, $"***************************************");
        }
      }
      catch (Exception e)
      {
        Log.Error(Instance, e);
      }
    }

    [UsedImplicitly] 
    public static bool DoesPluginExist(string pluginGuid) => Chainloader.PluginInfos.Any(keyValuePair => keyValuePair.Value.Metadata.GUID == pluginGuid);
    
    [UsedImplicitly] 
    public IEnumerable<FileInfo> GetModAssemblies() => Directory.GetFiles(Paths.PluginPath, "*.dll", SearchOption.AllDirectories).Select(filePath => new FileInfo(filePath));

    private IEnumerable<BepInExPluginInfoProxy> GetPlugins()
    {
      var pluginTypes = TypeLoader.FindPluginTypes(Paths.PluginPath, Chainloader.ToPluginInfo, cacheName: CacheName);
      foreach (var keyValuePair in pluginTypes)
      {
        foreach (var pluginInfo in keyValuePair.Value)
        {
          yield return new BepInExPluginInfoProxy(pluginInfo, keyValuePair.Key);
        }
      }
    }

    #region PluginInfoProxy

    private class BepInExPluginInfoProxy
    {
      private readonly BepInEx.PluginInfo _pluginInfo;

      public BepInExPluginInfoProxy(BepInEx.PluginInfo pluginInfo, string location)
      {
        _pluginInfo = pluginInfo;
        Location = location;
      }

      public BepInPlugin Metadata => _pluginInfo.Metadata;
      [UsedImplicitly] public IEnumerable<BepInProcess> Processes => _pluginInfo.Processes;
      [UsedImplicitly] public IEnumerable<BepInDependency> Dependencies => _pluginInfo.Dependencies;
      [UsedImplicitly] public IEnumerable<BepInIncompatibility> Incompatibilities => _pluginInfo.Incompatibilities;
      public string Location { get; }
      // ReSharper disable once MemberHidesStaticFromOuterClass
      [UsedImplicitly] public BaseUnityPlugin Instance => _pluginInfo.Instance;
      public override string ToString() => _pluginInfo.ToString();
    }

    #endregion
  }
}
