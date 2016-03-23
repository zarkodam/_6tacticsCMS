using _6tactics.Cms.Core.Entities;
using _6tactics.Cms.Core.Enums.Admin;

namespace _6tactics.Cms.Web.App_Logic
{
    public class TreeView
    {
        public static string SelectGroupColor
            (
                ContentItem currentContentItem,
                string projectColor,
                string languageColor,
                string pageColor,
                string footerColor,
                string contentElemmentColor,
                string subElementColor,
                string fileColor,
                string subFileColor
            )
        {
            if (currentContentItem.ContentType == ContentType.Language)
                return languageColor;
            if (currentContentItem.ContentType == ContentType.Page)
                return pageColor;
            if (currentContentItem.ContentType == ContentType.Footer)
                return footerColor;
            if (currentContentItem.ContentType == ContentType.ContentElement)
            {
                if (ElementsVisibilityUtility.IsTitleOfContent(currentContentItem))
                    return contentElemmentColor;
                return subElementColor;
            }
            if (currentContentItem.ContentType == ContentType.FileElement)
            {
                if (ElementsVisibilityUtility.IsTitleOfContent(currentContentItem))
                    return fileColor;
                return subFileColor;
            }

            return projectColor;
        }
    }
}
