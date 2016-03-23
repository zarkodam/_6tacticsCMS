using _6tactics.Cms.Core.Enums.Admin;
using System.Collections.Generic;

namespace _6tactics.Cms.Core.Helpers
{
    public static class DisplayTypeSelectorHelper
    {
        /*
         "ImageTopTitleSummaryContent",
         "TitleImageTopContent",
         "TitleContentImageBottom", 
         */

        private static readonly List<string> ContentElementDisplayTypes = new List<string>
        {
            "EmptyRow",

            "Bulletin",

            "Content",
            "ContentImageBottom",
            "ContentImageRight",

            "TitleContent",
            "TitleContentImageBottom",
            "TitleImageRightContent",
            "TitleImageRightSummaryContent",
            "TitleImageTopContent",
            "TitleImageTopSummaryContent",
            "TitleSummaryContent",
            "TitleSummaryContentImageBottom",

            "ImageLeftContent",
            "ImageLeftTitleContent",
            "ImageLeftTitleSummaryContent",

            "ImageTopContent",
            "ImageTopTitleContent",
            "ImageTopTitleSummaryContent",

            "CubeLink",
            "LinkWithImage",
            "LinkWithTitleAndImage",
            "LinkWithTitle",

            "YoutubeVideo",
            "VideoAsPopup",

            // Plugins
            "ParallaxSliderFluid",
            "GoogleMap",
            "GoogleMapFluid",
            "GoogleMapSummary",
            "SummaryGoogleMap",
            "ContactUsMailForm",
            "ContactUsMailFormSummary",
            "SummaryContactUsMailForm",
        };

        private static readonly List<string> PageDisplayTypes = new List<string>
        {
            "Normal" , "Popup"
        };


        private static readonly List<string> FileElementDisplayTypes = new List<string>
        {
            "Image", "ImageFluid", "ImageSlider", "ImageGallery", "FilesForDownload"
        };

        public static IEnumerable<string> DisplayTypesForCreatePage(ContentType selectedContentType)
        {
            switch (selectedContentType)
            {
                case ContentType.Language:
                    return PageDisplayTypes;
                case ContentType.Page:
                    return PageDisplayTypes;
                case ContentType.Footer:
                    return ContentElementDisplayTypes;
                case ContentType.ContentElement:
                    return ContentElementDisplayTypes;
                case ContentType.FileElement:
                    return FileElementDisplayTypes;
                default:
                    return null;
            }

        }
        public static IEnumerable<string> DisplayTypesForEditPage(ContentType selectedContentType)
        {
            switch (selectedContentType)
            {
                case ContentType.Page:
                    return PageDisplayTypes;
                case ContentType.ContentElement:
                    return ContentElementDisplayTypes;
                case ContentType.Footer:
                    return ContentElementDisplayTypes;
                case ContentType.FileElement:
                    return FileElementDisplayTypes;
                default:
                    return null;
            }
        }
    }
}
