using _6tactics.Cms.Core.ViewModels.Admin;
using _6tactics.Cms.Core.ViewModels.Web;
using Newtonsoft.Json;
using System;
using System.IO;

namespace _6tactics.Cms.Core.Utilities
{
    public class SeoUtility
    {
        private static readonly string AppBaseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        private static readonly string AppDataDirectory = Path.Combine(AppBaseDirectory, "App_Data");
        private static readonly string SeoConfigDataDirectory = Path.Combine(AppDataDirectory, "SEOConfigurationData");
        private static readonly string SeoDataJsonFile = Path.Combine(SeoConfigDataDirectory, "seoData.json");


        public static SeoViewModel GetSeoData()
        {
            return JsonConvert.DeserializeObject<SeoViewModel>(File.ReadAllText(SeoDataJsonFile));
        }

        public static void SaveSeoData(SeoViewModel seoViewModel)
        {
            File.WriteAllText(SeoDataJsonFile, JsonConvert.SerializeObject(seoViewModel));
        }

        public static void DeleteSeoData()
        {
            File.WriteAllText(SeoDataJsonFile, JsonConvert.SerializeObject(new SetupViewModel()));
        }
    }
}
