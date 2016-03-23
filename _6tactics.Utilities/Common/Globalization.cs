using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace _6tactics.Utilities.Common
{
    public class Globalization
    {
        public static Dictionary<string, string> GetLanguagesForDropDown()
        {
            // For example: English, EN
            var result = new Dictionary<string, string>();

            // Get all available cultures on the current system.
            var cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);

            foreach (var culture in cultures.OrderBy(i => i.DisplayName))
            {
                if (culture.NativeName.StartsWith("?"))
                    continue;

                if (culture.Name.Length > 2)
                    continue;

                //result.Add(culture.DisplayName, culture.Name);

                if (!result.ContainsKey(culture.DisplayName))
                    result.Add(culture.DisplayName, culture.TwoLetterISOLanguageName);

            }

            return result;
        }

        public static string GetDisplayNameByCultureName(string cultureName)
        {
            var cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
            var selectedCultureElement = cultures.FirstOrDefault(i => i.Name.Equals(cultureName, StringComparison.InvariantCultureIgnoreCase));

            return selectedCultureElement?.DisplayName;
        }
    }
}
