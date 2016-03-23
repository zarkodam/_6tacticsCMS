using System.Collections.Generic;

namespace _6tactics.Cms.Core.Entities
{
    public class Plugin
    {
        public int Id { get; set; }
        public string PluginName { get; set; }
        public ICollection<PluginSettings> PluginSettings { get; set; }
    }
}
