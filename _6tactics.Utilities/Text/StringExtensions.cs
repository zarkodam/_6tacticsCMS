using System;
using System.Linq;

namespace _6tactics.Utilities.Text
{
    public static class StringExtensions
    {
        public static string AddSpaceBeforeUpper(this string source)
        {
            return string.Concat(source.Select(x => Char.IsUpper(x) ? " " + x : x.ToString())).TrimStart(' ');
        }
    }
}
