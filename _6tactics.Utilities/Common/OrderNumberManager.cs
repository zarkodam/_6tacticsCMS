using System.Globalization;

namespace _6tactics.Utilities.Common
{
    public static class OrderNumberManager
    {
        public static string Create(uint numb)
        {
            return numb <= 9 ? string.Concat("0", numb) : numb.ToString(CultureInfo.InvariantCulture);
        }

        public static string CreateForCredits(double numb)
        {
            return numb >= 1 && numb < 10 && !numb.ToString(CultureInfo.InvariantCulture).Contains(",5")
                ? string.Concat("0", numb)
                : numb.ToString(CultureInfo.InvariantCulture);
        }
    }
}
