
namespace _6tactics.Utilities.Common
{
    public static class EvenOdd
    {
        public static bool IsOdd(int value)
        {
            return value % 2 != 0;
        }

        public static bool IsEven(int value)
        {
            return value % 2 == 0;
        }

        public static bool IsZero(int value)
        {
            return value == 0;
        }
    }
}
