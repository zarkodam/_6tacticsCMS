using System;

namespace _6tactics.Utilities.DateAndTime
{
    public static class DateTimeParse
    {
        public static DateTime Parse(string dateTimeFromString)
        {
            return DateTime.ParseExact(dateTimeFromString, "dd.MM.yyyy", null);
        }
    }
}
