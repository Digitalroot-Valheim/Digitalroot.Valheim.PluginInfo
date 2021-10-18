﻿using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Configuration;
using Digitalroot.Valheim.Common;
using HarmonyLib;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Digitalroot.Valheim.PluginInfo
{
  [BepInPlugin(Guid, Name, Version)]
  [BepInDependency(JVLGuid, BepInDependency.DependencyFlags.SoftDependency)]
  public class Main : BaseUnityPlugin, ITraceableLogging
  {
    private Harmony _harmony;
    public const string Version = "1.3.1";
    public const string Name = "Digitalroot Plug-in Info";
    // ReSharper disable once MemberCanBePrivate.Global
    public const string Guid = "digitalroot.mods.plugininfo";
    public const string Namespace = "Digitalroot.Valheim." + nameof(PluginInfo);
    [UsedImplicitly] public static ConfigEntry<int> NexusId;
    public static Main Instance;
    private const string JVLGuid = "com.jotunn.jotunn";

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
        Log.Trace(Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
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
        Log.Trace(Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
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
        Log.Trace(Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
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
        Log.Trace(Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
        Log.Debug(Instance, "******* [Digitalroot Plug-ins Loaded] *******");
        foreach (KeyValuePair<string, BepInEx.PluginInfo> pluginInfo in BepInEx.Bootstrap.Chainloader.PluginInfos)
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
        foreach (string dependencyError in BepInEx.Bootstrap.Chainloader.DependencyErrors)
        {
          Log.Debug(Instance, $"{dependencyError}");
        }

        Log.Debug(Instance, $"***************************************");

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

    public static bool DoesPluginExist(string pluginGuid) => Chainloader.PluginInfos.Any(keyValuePair => keyValuePair.Value.Metadata.GUID == pluginGuid);

    private void HandleJVLData()
    {
      try
      {
        Log.Trace(Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");

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
  }
}
