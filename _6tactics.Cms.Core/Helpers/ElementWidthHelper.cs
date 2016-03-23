using System.Collections.Generic;

namespace _6tactics.Cms.Core.Helpers
{
    public class ElementWidthHelper
    {
        public static IEnumerable<string> AsBootstrapClass()
        {
            return new[]
            {
                "col-md-12", 
                //"col-md-11", 
                //"col-md-10", 
                //"col-md-9", 
                //"col-md-8", 
                //"col-md-7", 
                "col-md-6",
                //"col-md-5",
                "col-md-4",
                "col-md-3",
                "col-md-2",
                "col-md-1",
                "col-md-11 centered",
                "col-md-10 centered",
                "col-md-9 centered",
                "col-md-8 centered",
                "col-md-7 centered",
                "col-md-6 centered",
                //"col-md-5 centered",
                //"col-md-4 centered",
                //"col-md-3 centered",
                //"col-md-2 centered",
                //"col-md-1 centered"
            };
        }

        public static IEnumerable<string> AsPercentage()
        {
            return new[]
            {
                "100%",
                "91.66666667%",
                "83.33333333%",
                "75%",
                "66.66666667%",
                "58.33333333%",
                "50%",
                "41.66666667%",
                "33.33333333%",
                "25%",
                "16.66666667%",
                "8.33333333%"
            };
        }
    }
}

//cetered