# PluginInfo

## License
Closed-source license is available for commercial use.

### PluginInfo
Writes output to the LogOutput.log detailing mods loaded by <a href="https://harmony.pardeike.net/"  target="_blank">Harmony</a>. 
Any dependency errors are listed at the bottom of the output.

#### Dependencies
- <a href="https://valheim.thunderstore.io/package/denikson/BepInExPack_Valheim/"  target="_blank">BepInExPack Valheim</a>

#### Installation (manual)
1. Download the latest release archive (zip) file from https://github.com/Digitalroot/digitalroot-valheim-mods/releases/latest
1. Extract the archive into &lt;Steam Location&gt;\steamapps\common\Valheim\BepInEx\plugins

#### Configuration 
- No need to configure.

### Crossplay <span class="checked">✔</span>
| Point of View               | Server w/ Mod                   | Server w/o Mod                  | Client (PC) w/ mod              | Client (PC/Console) w/o mod     |
| ---                         | ---                             | ---                             | ---                             | ---                             |
| Client (PC) w/ mod          | <span class="checked">✔</span> | <span class="checked">✔</span> | <span class="checked">✔</span> | <span class="checked">✔</span> |
| Client (PC/Console) w/o mod | <span class="checked">✔</span> | <span class="checked">✔</span> | <span class="checked">✔</span> | <span class="checked">✔</span> |
| Server w/ Mod               |                                 |                                 | <span class="checked">✔</span> | <span class="checked">✔</span> |
| Server w/o Mod              |                                 |                                 | <span class="checked">✔</span> | <span class="checked">✔</span> |

#### Example Output
```
[Debug  :Digitalroot] ******* [Digitalroot Plug-ins Loaded] *******
[Debug  :Digitalroot] Key: com.jotunn.jotunn
[Debug  :Digitalroot] Value: Jotunn 2.0.6
[Debug  :Digitalroot] GUID: com.jotunn.jotunn
[Debug  :Digitalroot] Name: Jotunn
[Debug  :Digitalroot] Version: 2.0.6
[Debug  :Digitalroot] Location: C:\Program Files (x86)\Steam\steamapps\common\Valheim\BepInEx\plugins\Jotunn\Jotunn.dll
[Debug  :Digitalroot] Dependencies:
[Debug  :Digitalroot] Incompatibilities:
[Debug  :Digitalroot] Instance: BepInEx_Manager (Jotunn.Main)
[Debug  :Digitalroot] ***************************************
[Debug  :Digitalroot] Key: maximods.valheim.multicraft
[Debug  :Digitalroot] Value: MultiCraft 0.2.6
[Debug  :Digitalroot] GUID: maximods.valheim.multicraft
[Debug  :Digitalroot] Name: MultiCraft
[Debug  :Digitalroot] Version: 0.2.6
[Debug  :Digitalroot] Location: C:\Program Files (x86)\Steam\steamapps\common\Valheim\BepInEx\plugins\MaxiMods-MultiCraft\MultiCraft.dll
[Debug  :Digitalroot] Dependencies:
[Debug  :Digitalroot] DependencyGUID: aedenthorn.CraftFromContainers
[Debug  :Digitalroot] Flags: SoftDependency
[Debug  :Digitalroot] MinimumVersion: 0.0
[Debug  :Digitalroot] Incompatibilities:
[Debug  :Digitalroot] Instance: BepInEx_Manager (MultiCraft.MultiCraftPlugin)
[Debug  :Digitalroot] ***************************************
[Debug  :Digitalroot] DependencyErrors
[Debug  :Digitalroot] ***************************************
```

## Thanks to 
- <a href="https://github.com/RandyKnapp" target="_blank">RandyKnapp</a> and <a href="https://github.com/Menthus123" target="_blank">Menthus</a> for answering my questions.
- The <a href="https://github.com/Valheim-Modding/Jotunn" target="_blank">Jotunn</a> crew for creating great examples.
- The <a href="https://discord.gg/GUEBuCuAMz" target="_blank">Valheim Discord</a> community. 
<br />

<p align="center">
<b>Digitalroot can be found in the Valhalla Legends Discord</b><br /><br />
  <a href="https://discord.gg/SsMW3rm67u" target="_blank"><img src="https://digitalroot.net/img/vl/vl_logo_125x154.png"></a>
</p>
