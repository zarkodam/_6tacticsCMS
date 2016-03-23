using System;

namespace _6tactics.Utilities.DateAndTime
{
    public static class TimeCompare
    {
        public static bool IsLower(TimeSpan timeA, TimeSpan timeB)
        {
            return DateTime.Now.Add(timeA).TimeOfDay < timeB;
        }

        public static bool IsGreaterOrEqual(TimeSpan timeA, TimeSpan timeB)
        {
            return timeA >= timeB;
        }
    }
}
