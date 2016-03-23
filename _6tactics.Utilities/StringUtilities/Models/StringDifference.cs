namespace _6tactics.Utilities.StringUtilities.Models
{
    public class StringDifference
    {
        public string OriginalSource { get; set; }
        public string[] PreparedSource { get; set; }
        public string[] PreparedTarget { get; set; }
        public string[] DifferenceFromSource { get; set; }
        public string[] DifferenceFromTarget { get; set; }
    }
}
