using _6tactics.Cms.Core.Entities;
using _6tactics.Cms.Core.ViewModels.PluginsManager;
using _DataAccess.UnitOfWork;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Plugin = _6tactics.Cms.Core.ViewModels.PluginsManager.Plugin;

namespace _6tactics.Cms.Services.Plugins
{
    public class PluginsManagerService : IPluginsManagerService
    {
        #region Fields

        private readonly IUnitOfWork _uof;
        private static readonly string PluginsFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Areas", "Plugins");
        private static readonly string PluginDataFolder = Path.Combine(PluginsFolder, "PluginData");

        #endregion


        #region Constructors

        public PluginsManagerService(IUnitOfWork uof)
        {
            _uof = uof;
        }

        #endregion


        #region GetPluginsAndTheirActions

        public IEnumerable<Plugin> GetListOfPluginsWithActions()
        {
            var pluginsList = new List<Plugin>();

            foreach (var folder in Directory.EnumerateDirectories(PluginsFolder))
                foreach (var file in Directory.EnumerateFiles(folder, "*.cs"))
                    pluginsList.Add(new Plugin
                    {
                        RouteName = GetFileNameWithoutExtension(file),
                        PluginActions = GetListOfPluginActions(file, GetFileNameWithoutExtension(file))
                    });

            return pluginsList;
        }

        public Plugin GetPluginActions(string pluginName)
        {
            foreach (var folder in Directory.EnumerateDirectories(PluginsFolder))
                foreach (var file in Directory.EnumerateFiles(folder, "*.cs"))
                    if (GetFileNameWithoutExtension(file).Equals(pluginName))
                        return new Plugin
                        {
                            RouteName = GetFileNameWithoutExtension(file),
                            PluginActions = GetListOfPluginActions(file, GetFileNameWithoutExtension(file))
                        };

            return null;
        }

        #endregion


        #region SerializationUtilities

        public T DeserializeModelFromJson<T>(string jsonString) where T : class
        {
            string toSerialize = string.IsNullOrEmpty(jsonString) || jsonString.Equals("{}") ? "" : jsonString;
            return JsonConvert.DeserializeObject<T>(toSerialize);
        }

        public string SerializeModelAsJson(object objectToSerialze, Formatting formatting = Formatting.Indented)
        {
            return JsonConvert.SerializeObject(objectToSerialze, formatting);
        }

        #endregion


        #region ReadWriteToJsonToDbUtilities

        public string GetPluginDataAsJsonStringFromDb(string pluginName, string settingName)
        {
            var pluginSettingsRepo = _uof.GenericRepo<PluginSettings>();

            bool isPluginExist = pluginSettingsRepo.Get().Any(i => i.Plugin.PluginName.Equals(pluginName));

            if (!isPluginExist) return null;

            var settings = pluginSettingsRepo.Get().FirstOrDefault(i => i.Plugin.PluginName.Equals(pluginName) && i.SettingsName.Equals(settingName));

            return settings != null ? settings.SettingsJsonString : "{}";
        }

        public T GetPluginDataFromDb<T>(string pluginName, string settingName) where T : class
        {
            return DeserializeModelFromJson<T>(GetPluginDataAsJsonStringFromDb(pluginName, settingName));
        }

        public void SavePluginDataToDb<T>(string pluginName, string settingName, T objectToSerialze) where T : class
        {
            var pluginRepo = _uof.GenericRepo<Core.Entities.Plugin>();

            bool isPluginExist = pluginRepo.Get().Any(i => i.PluginName.Equals(pluginName));

            if (isPluginExist)
            {
                var plugin = pluginRepo.Get().First(i => i.PluginName.Equals(pluginName));

                if (plugin.PluginSettings.Any(i => i.SettingsName.Equals(settingName)))
                {
                    var settings = plugin.PluginSettings.First(i => i.SettingsName.Equals(settingName));
                    settings.SettingsJsonString = SerializeModelAsJson(objectToSerialze, Formatting.None);
                }
                else
                {
                    plugin.PluginSettings.Add(new PluginSettings
                    {
                        SettingsName = settingName,
                        SettingsJsonString = SerializeModelAsJson(objectToSerialze, Formatting.None)
                    });
                }

                pluginRepo.Update(plugin);
            }
            else
            {
                var plugin = new Core.Entities.Plugin
                {
                    PluginName = pluginName,
                    PluginSettings = new List<PluginSettings>
                    {
                        new PluginSettings
                        {
                            SettingsName = settingName,
                            SettingsJsonString = SerializeModelAsJson(objectToSerialze, Formatting.None)
                        }
                    }
                };

                pluginRepo.Insert(plugin);
            }

            _uof.Save();
        }


        #endregion


        #region ReadWriteJsonToFileUtilities

        public string GetPluginDataAsJsonStringFromFile(string pluginName, string settingName)
        {
            string pluginDataFolder = Path.Combine(PluginDataFolder, pluginName);

            if (!Directory.Exists(pluginDataFolder)) return null;

            string pluginDataFile = Path.Combine(pluginDataFolder, string.Concat(settingName, ".json"));

            return File.Exists(pluginDataFile)
                ? File.ReadAllText(pluginDataFile)
                : null;
        }

        public T GetPluginDataFromFile<T>(string pluginName, string settingName) where T : class
        {
            string pluginDataFolder = Path.Combine(PluginDataFolder, pluginName);

            if (!Directory.Exists(pluginDataFolder)) return null;

            string pluginDataFile = Path.Combine(pluginDataFolder, string.Concat(settingName, ".json"));

            return File.Exists(pluginDataFile)
                ? DeserializeModelFromJson<T>(File.ReadAllText(pluginDataFile))
                : null;
        }

        public void SavePluginDataToFile<T>(string pluginName, string settingName, T objectToSerialze) where T : class
        {
            string pluginDataFolder = Path.Combine(PluginDataFolder, pluginName);

            if (!Directory.Exists(pluginDataFolder))
                Directory.CreateDirectory(pluginDataFolder);

            string pluginDataFile = Path.Combine(pluginDataFolder, string.Concat(settingName, ".json"));

            if (!File.Exists(pluginDataFile))
                File.Create(pluginDataFile).Close();

            File.WriteAllText(pluginDataFile, SerializeModelAsJson(pluginDataFile));
        }

        #endregion


        #region GetAndSetSettings

        public string GetPluginDataAsJsonString(string pluginName, string settingName)
        {
            return GetPluginDataAsJsonStringFromDb(pluginName, settingName);
            //return GetPluginDataAsJsonStringFromFile(pluginName, settingName);
        }

        public T GetPluginData<T>(string pluginName, string settingName) where T : class
        {
            return GetPluginDataFromDb<T>(pluginName, settingName);
            //return GetPluginDataFromFile<T>(pluginName, settingName);
        }

        public void SavePluginData<T>(string pluginName, string settingName, T objectToSerialze) where T : class
        {
            SavePluginDataToDb<T>(pluginName, settingName, objectToSerialze);
            //SavePluginDataToFile<T>(pluginName, settingName, objectToSerialze);
        }

        #endregion


        #region Helpers

        private string GetFileNameWithoutExtension(string filename)
        {
            return string.IsNullOrEmpty(filename)
                ? null
                : Path.GetFileName(filename).Replace(Path.GetExtension(filename), "").Replace("Controller", "");
        }

        private bool ExcludeMethodsWithAttribute(string line, string attributeName)
        {
            return line.ToCharArray().Any(i => i == '[' || i == ']') && line.Contains(attributeName);
        }

        private IEnumerable<string> FileteredClassFile(string file)
        {
            IEnumerable<string> publicMethods = File.ReadAllLines(file);

            var filteredContent = new List<string>();

            bool isMethodPost = false;

            foreach (string line in publicMethods)
            {
                if (ExcludeMethodsWithAttribute(line, "HttpPost") || ExcludeMethodsWithAttribute(line, "ExcludeFromPluginActionList"))
                    isMethodPost = true;

                if (!isMethodPost && line.Contains("public") && !line.Contains("Controller"))
                    filteredContent.Add(line);

                if (isMethodPost && line.Contains("public") && !line.Contains("Controller"))
                    isMethodPost = false;
            }

            return filteredContent;
        }

        private IEnumerable<PluginAction> GetListOfPluginActions(string file, string className, bool exceptConstructors = true)
        {
            List<string> methodNames = new List<string>();

            foreach (var method in FileteredClassFile(file).Where(i => i.Contains("(")))
            {
                string methodNameWithParameters = method.Split(' ').First(i => i.Contains("("));
                string mehodNameWithoutParameters = methodNameWithParameters.Substring(0,
                    methodNameWithParameters.IndexOf("(", StringComparison.Ordinal));

                if (exceptConstructors && className.Equals(mehodNameWithoutParameters, StringComparison.InvariantCultureIgnoreCase))
                    methodNames.Add(mehodNameWithoutParameters);
                else
                    methodNames.Add(mehodNameWithoutParameters);
            }

            return methodNames.Distinct().Select(method => new PluginAction { RouteName = method }).ToList();
        }

        #endregion
    }
}
