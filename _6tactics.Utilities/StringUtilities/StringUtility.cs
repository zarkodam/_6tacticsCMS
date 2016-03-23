namespace _6tactics.Utilities.StringUtilities
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

        public static string UppercaseFirst(string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;
            return char.ToUpper(text[0]) + text.Substring(1);
        }

        public static string CreateOrderNumber(int numb)
        {
            return numb <= 9 ? string.Concat("0", numb) : numb.ToString();
        }

        public static string CreateOrderNumber(uint numb)
        {
            return numb <= 9 ? string.Concat("0", numb) : numb.ToString();
        }
    }
}
