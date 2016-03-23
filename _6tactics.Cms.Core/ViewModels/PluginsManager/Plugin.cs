using _6tactics.Utilities.Text;
using System.Collections.Generic;

namespace _6tactics.Cms.Core.ViewModels.PluginsManager
{
    public class Plugin
    {
        public string RouteName { get; set; }
        public string PreviewName => RouteName.AddSpaceBeforeUpper();
        public IEnumerable<PluginAction> PluginActions { get; set; }
    }
}
