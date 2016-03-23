namespace _6tactics.Cms.Core.Entities
{
    public class PluginSettings
    {
        public int Id { get; set; }
        public string SettingsName { get; set; }
        public string SettingsJsonString { get; set; }
        public Plugin Plugin { get; set; }
    }
}
