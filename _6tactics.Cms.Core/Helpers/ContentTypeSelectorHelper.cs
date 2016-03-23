using _6tactics.Cms.Core.Enums.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace _6tactics.Cms.Core.Helpers
{
    public static class ContentTypeSelectorHelper
    {
        public static IEnumerable<string> GetChildTypes(ContentType selectedContentType)
        {
            var contentTypes = Enum.GetNames(typeof(ContentType));

            switch (selectedContentType)
            {
                case ContentType.Project:
                    return contentTypes.Where(i => i == "Language");
                case ContentType.Language:
                    return contentTypes.Where(i => i == "Page" || i == "Footer");
                case ContentType.Page:
                    return contentTypes.Where(i => i == "Page" || i == "ContentElement" || i == "FileElement");
                case ContentType.Footer:
                    return contentTypes.Where(i => i == "ContentElement" || i == "FileElement");
                case ContentType.ContentElement:
                    return contentTypes.Where(i => i == "ContentElement");
                case ContentType.FileElement:
                    return contentTypes.Where(i => i == "FileElement");
                default:
                    return contentTypes;
            }
        }

        public static IEnumerable<string> GetChildTypesForEditing(ContentType selectedContentType)
        {
            var contentTypes = Enum.GetNames(typeof(ContentType));

            switch (selectedContentType)
            {
                case ContentType.Project:
                    return contentTypes.Where(i => i == "Project");
                case ContentType.Language:
                    return contentTypes.Where(i => i == "Language");
                case ContentType.Page:
                    return contentTypes.Where(i => i == "Page");
                case ContentType.Footer:
                    return contentTypes.Where(i => i == "Footer");
                case ContentType.ContentElement:
                    return contentTypes.Where(i => i == "ContentElement" || i == "FileElement");
                case ContentType.FileElement:
                    return contentTypes.Where(i => i == "FileElement" || i == "ContentElement");
                default:
                    return contentTypes;
            }
        }

        public static IEnumerable<SelectListItem> GetChildContentTypesList(ContentType selectedContentType)
        {
            return GetChildTypes(selectedContentType).Select(text => new SelectListItem { Text = text });
        }

        public static IEnumerable<SelectListItem> GetChildContentTypesListForEditing(ContentType selectedContentType)
        {
            return GetChildTypesForEditing(selectedContentType).Select(text => new SelectListItem { Text = text });
        }

        public static bool IsDropDownVisible(ContentType selectedContentType)
        {
            return GetChildContentTypesList(selectedContentType).ToList().Count > 1;
        }

        public static bool IsDropDownForEditingVisible(ContentType selectedContentType)
        {
            return GetChildContentTypesListForEditing(selectedContentType).ToList().Count > 1;
        }
    }
}
