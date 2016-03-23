using _6tactics.Utilities.StringUtilities.Models;
using FuzzyString;
using StringMatching;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace _6tactics.Utilities.StringUtilities
{
    public static class StringExtensions
    {
        // Checks is string contains value with StringComparison.Ordinal
        public static bool CustomContains(this string source, string toCheck)
        {
            return source.IndexOf(toCheck, StringComparison.Ordinal) >= 0;
        }

        // Checks is string contains value with custom StringComparison
        public static bool CustomContains(this string source, string toCheck, StringComparison stringComparison)
        {
            return source.IndexOf(toCheck, stringComparison) >= 0;
        }

        private static bool IsSimilarCandidate(string a, string b)
        {
            char[] bChars = b.ToCharArray();

            return a.Equals(b) || a.Length == b.Length ||
                (a.StartsWith(bChars.First().ToString(CultureInfo.InvariantCulture)) ||
                a.EndsWith(bChars.Last().ToString(CultureInfo.InvariantCulture)));
        }

        // Returns character difference
        public static List<CharDifference> GetCharDifference(this string a, string b)
        {
            char[] aSource = a.ToCharArray();
            char[] bSource = b.ToCharArray();

            char[] differenceFromASource = aSource.Except(bSource).ToArray();
            char[] differenceFromBSource = bSource.Except(aSource).ToArray();

            var charDifference = new List<CharDifference>();

            if (differenceFromBSource.Length <= differenceFromASource.Length)
                for (int i = 0; i < differenceFromBSource.Length; i++)
                    charDifference.Add(new CharDifference { Bad = differenceFromBSource[i], Good = differenceFromASource[i] });

            return charDifference;
        }

        // Get string difference percentage 
        public static double GetStringDifferencePercentage(this string source, string target)
        {
            var stringMatching = new FuzzyStringMatching();
            return Math.Round(stringMatching.GetSimilarity(source, target) * 100, 2);
        }

        // Similar candidates dictionary string(song name) int(percentage) collection 
        public static void SimilarCandidates(this IEnumerable<string> sourceCollection, IEnumerable<string> targetCollection)
        {
            // ...
        }

        // Preparing source and target strings with regex for comparison
        public static PreparedStrings PrepareStrings(this string source, string target, Regex regex)
        {
            //Regex regex = new Regex("[^a-zA-Z0-9 -]");

            return new PreparedStrings
            {
                PreparedSource = regex.Replace(source, "").Split(' '),
                PreparedTarget = regex.Replace(target, "").Split(' ')
            };
        }

        // Get different words from source and target strings
        public static StringDifference GetStringDifference(this PreparedStrings preparedStrings, string target, Regex regex)
        {
            return new StringDifference
            {
                PreparedSource = preparedStrings.PreparedSource,
                PreparedTarget = preparedStrings.PreparedTarget,
                DifferenceFromSource = preparedStrings.PreparedSource.Except(preparedStrings.PreparedTarget).ToArray(),
                DifferenceFromTarget = preparedStrings.PreparedTarget.Except(preparedStrings.PreparedSource).ToArray()
            };
        }

        // Fixing source by comparing two strings with FuzzyString library
        public static bool IsStringSimmlarWith(this string source, string target)
        {
            var options = new List<FuzzyStringComparisonOptions>
            {
                // https://en.wikipedia.org/wiki/Overlap_coefficient
                FuzzyStringComparisonOptions.UseOverlapCoefficient,
                // https://en.wikipedia.org/wiki/Longest_common_subsequence_problem
                FuzzyStringComparisonOptions.UseLongestCommonSubsequence,
                // https://en.wikipedia.org/wiki/Longest_common_substring_problem
                FuzzyStringComparisonOptions.UseLongestCommonSubstring
            };

            const FuzzyStringComparisonTolerance tolerance = FuzzyStringComparisonTolerance.Normal;

            return source.ApproximatelyEquals(target, options, tolerance);
        }

        public static bool IsStringSimmlarWith(this string source, string target, List<FuzzyStringComparisonOptions> options, FuzzyStringComparisonTolerance tolerance)
        {
            return source.ApproximatelyEquals(target, options, tolerance);
        }

        // Fixing source by words from source string with FuzzyString library
        public static string FixSourceByWords(this PreparedStrings preparedStrings)
        {
            var options = new List<FuzzyStringComparisonOptions>
            {
                // https://en.wikipedia.org/wiki/Overlap_coefficient
                FuzzyStringComparisonOptions.UseOverlapCoefficient,
                // https://en.wikipedia.org/wiki/Longest_common_subsequence_problem
                FuzzyStringComparisonOptions.UseLongestCommonSubsequence,
                // https://en.wikipedia.org/wiki/Longest_common_substring_problem
                FuzzyStringComparisonOptions.UseLongestCommonSubstring
            };

            const FuzzyStringComparisonTolerance tolerance = FuzzyStringComparisonTolerance.Normal;

            string possibleName = "";

            foreach (string sourceWord in preparedStrings.PreparedSource)
                foreach (string targetWord in preparedStrings.PreparedTarget)
                {

                    bool result = sourceWord.ApproximatelyEquals(targetWord, options, tolerance);

                    if (!result) continue;

                    possibleName += possibleName.Length > 0 ? " " + targetWord : "" + targetWord;

                    break;
                }

            return possibleName;
        }

        public static string FixSourceByWords(this PreparedStrings preparedStrings, List<FuzzyStringComparisonOptions> options, FuzzyStringComparisonTolerance tolerance)
        {
            string possibleName = "";

            foreach (string sourceWord in preparedStrings.PreparedSource)
                foreach (string targetWord in preparedStrings.PreparedTarget)
                {
                    bool result = sourceWord.ApproximatelyEquals(targetWord, options, tolerance);

                    if (!result) continue;

                    possibleName += possibleName.Length > 0 ? " " + targetWord : "" + targetWord;

                    break;
                }

            return possibleName;
        }
    }
}


/*
Usage example:

const string a = "Walk in tHe Sky (feat. Bajka)";
const string b = "asdasd Wal0 in tge Sjy 1";

var regex = new Regex("[^a-zA-Z0-9 -]", RegexOptions.Compiled);

var options = new List<FuzzyStringComparisonOptions>
{
    FuzzyStringComparisonOptions.UseLongestCommonSubsequence,
};

const FuzzyStringComparisonTolerance tolerance = FuzzyStringComparisonTolerance.Normal;

Console.WriteLine(b.PrepareStrings(a, regex).FixSource(options, tolerance));

*/
