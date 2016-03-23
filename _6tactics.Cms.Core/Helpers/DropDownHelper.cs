using _6tactics.Cms.Core.Enums.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace _6tactics.Cms.Core.Helpers
{
    public class DropDownHelper
    {
        // ContentType
        public static IEnumerable<SelectListItem> GetChildContentTypesList(ContentType selectedContentType)
        {
            return ContentTypeSelectorHelper.GetChildTypes(selectedContentType).Select(text => new SelectListItem { Text = text });
        }

        public static IEnumerable<SelectListItem> GetChildContentTypesListForEditing(ContentType selectedContentType)
        {
            return ContentTypeSelectorHelper.GetChildTypesForEditing(selectedContentType).Select(text => new SelectListItem { Text = text });
        }

        public static bool IsDropDownVisible(ContentType selectedContentType)
        {
            return GetChildContentTypesList(selectedContentType).ToList().Count > 1;
        }

        public static bool IsDropDownForEditingVisible(ContentType selectedContentType)
        {
            return GetChildContentTypesListForEditing(selectedContentType).ToList().Count > 1;
        }

        // DisplayType
        public static IEnumerable<SelectListItem> GetDisplayTypeListForCreatePage(ContentType contentType)
        {
            IEnumerable<string> displayTypes = DisplayTypeSelectorHelper.DisplayTypesForCreatePage(contentType);
            return displayTypes?.Select(text => new SelectListItem { Text = text });
        }

        public static IEnumerable<SelectListItem> GetDisplayTypeListForEditPage(ContentType contentType)
        {
            IEnumerable<string> displayTypes = DisplayTypeSelectorHelper.DisplayTypesForEditPage(contentType);
            return displayTypes?.Select(text => new SelectListItem { Text = text });
        }

        // Link options
        public static IEnumerable<SelectListItem> GetLinkOptionsList
        {
            get
            {
                var visibilityOptions = Enum.GetNames(typeof(LinkOption));
                return visibilityOptions.Select(text => new SelectListItem { Text = text });

            }
        }

        // Element visibility
        public static IEnumerable<SelectListItem> GetElementVisibilityList
        {
            get
            {
                var elementVisibilityOptions = Enum.GetNames(typeof(ElementVisibility));
                return elementVisibilityOptions.Select(text => new SelectListItem { Text = text });

            }
        }

        // Section title visibility
        public static IEnumerable<SelectListItem> GetSectionTitleVisibilityList
        {
            get
            {
                var elementVisibilityOptions = Enum.GetNames(typeof(SectionTitleVisibility));
                return elementVisibilityOptions.Select(text => new SelectListItem { Text = text });
            }
        }

        // Element width
        public static IEnumerable<SelectListItem> GetElementWidthList
        {
            get
            {
                return ElementWidthHelper.AsBootstrapClass().Select(text => new SelectListItem { Text = text });

            }
        }
    }
}
