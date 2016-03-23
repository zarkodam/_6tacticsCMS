using System.Collections.Generic;

namespace _6tactics.Utilities.StringUtilities.Models
{
    public class WordDifference
    {
        public StringDifference StringDifference { get; set; }
        public string Bad { get; set; }
        public string Good { get; set; }
        public List<CharDifference> CharDifference { get; set; }
    }
}
