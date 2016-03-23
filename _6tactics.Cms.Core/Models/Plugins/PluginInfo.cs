using System.Collections.Generic;

namespace _6tactics.Cms.Core.Models.Plugins
{
    public class PluginInfo : IPluginInfo
    {
        public string MainControllerName { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public List<string> SettingsFilesNames { get; set; }
    }
}