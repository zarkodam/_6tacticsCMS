using _6tactics.Utilities.Text;

namespace _6tactics.Cms.Core.ViewModels.PluginsManager
{
    public class PluginAction
    {
        public string RouteName { get; set; }
        public string PreviewName => RouteName.AddSpaceBeforeUpper();
    }
}
