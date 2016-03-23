using System.Globalization;
using System.IO;
using System.Linq;

namespace _6tactics.Utilities.Common
{
    public static class StringUtility
    {
        public static string Cut(string forCut, int cutLength, string endingText)
        {
            return forCut.Length >= cutLength ? string.Concat(forCut.Substring(0, cutLength), endingText) : forCut;
        }

        public static void TryToGetIntFromString(string stringInput, out string stringOut, out int? intOut)
        {
            stringOut = null;
            intOut = null;

            if (string.IsNullOrWhiteSpace(stringInput)) return;
            int parmValue;
            var isInt = int.TryParse(stringInput, out parmValue);

            if (isInt)
                intOut = parmValue;
            else
                stringOut = stringInput;
        }

        public static string RemoveIllegalCharacters(string fileName)
        {
            var defaultIllegals = Path.GetInvalidFileNameChars();
            var customIllegals = new[] { "!", "#", "$", "%", "&", "=", "*", "/", ";", " " };

            var withoutCustomIllegals = customIllegals.Aggregate(fileName,
                (current, c) => current.Replace(c.ToString(CultureInfo.InvariantCulture), string.Empty));

            var withoutDefaultIllegals = defaultIllegals.Aggregate(withoutCustomIllegals,
                (current, c) => current.Replace(c.ToString(CultureInfo.InvariantCulture), string.Empty));

            return withoutDefaultIllegals;
        }
    }
}
