using _6tactics.Cms.Core.ViewModels.PluginsManager;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace _6tactics.Cms.Services.Plugins
{
    public interface IPluginsManagerService
    {
        IEnumerable<Plugin> GetListOfPluginsWithActions();
        Plugin GetPluginActions(string pluginName);

        T DeserializeModelFromJson<T>(string jsonString) where T : class;
        string SerializeModelAsJson(object objectToSerialze, Formatting formatting = Formatting.Indented);

        string GetPluginDataAsJsonString(string pluginName, string settingName);
        T GetPluginDataFromFile<T>(string pluginName, string settingName) where T : class;
        void SavePluginDataToFile<T>(string pluginName, string settingName, T objectToSerialze) where T : class;

        T GetPluginDataFromDb<T>(string pluginName, string settingName) where T : class;
        void SavePluginDataToDb<T>(string pluginName, string settingName, T objectToSerialze) where T : class;

        T GetPluginData<T>(string pluginName, string settingName) where T : class;
        void SavePluginData<T>(string pluginName, string settingName, T objectToSerialze) where T : class;
    }
}