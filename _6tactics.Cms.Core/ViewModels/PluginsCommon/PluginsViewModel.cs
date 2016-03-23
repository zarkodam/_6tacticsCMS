using _6tactics.Cms.Core.Models.Plugins;
using System.Collections.Generic;

namespace _6tactics.Cms.Core.ViewModels.PluginsCommon
{
    public class PluginsViewModel
    {
        public IEnumerable<PluginInfo> PluginsList { get; set; }
        public PluginInfo SelectedPlugin { get; set; }
    }
}
