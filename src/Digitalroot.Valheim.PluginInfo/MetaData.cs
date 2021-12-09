namespace Digitalroot.Valheim.PluginInfo
{
  public partial class Main
  {
    #if DEBUG
    public const string Version = "1.4.1";
    #else
    public const string Version = "%%VERSION%%";
    #endif
    
    public const string Name = "Digitalroot Plug-in Info";
    // ReSharper disable once MemberCanBePrivate.Global
    public const string Guid = "digitalroot.mods.plugininfo";
    public const string Namespace = "Digitalroot.Valheim." + nameof(PluginInfo);
  }
}
