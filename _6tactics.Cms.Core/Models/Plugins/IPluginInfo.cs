using System.Collections.Generic;

namespace _6tactics.Cms.Core.Models.Plugins
{
    public interface IPluginInfo
    {
        string MainControllerName { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        string ShortDescription { get; set; }
        List<string> SettingsFilesNames { get; set; }
    }
}
