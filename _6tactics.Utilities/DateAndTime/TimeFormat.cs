using System;

namespace _6tactics.Utilities.DateAndTime
{
    public static class TimeFormat
    {
        public static string Format(TimeSpan time, bool forceZero = false)
        {
            if (time == TimeSpan.Zero)
                return forceZero ? "00:00" : "--:--";
            return string.Concat((time.Minutes < 10 ? "0" : ""), time.Minutes, ":", (time.Seconds < 10 ? "0" : ""), time.Seconds);
        }

        public static string Format(TimeSpan time1, TimeSpan time2)
        {
            return string.Concat(Format(time1), " / ", Format(time2));
        }
    }
}
