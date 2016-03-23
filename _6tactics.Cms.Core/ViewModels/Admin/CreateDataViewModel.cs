using _6tactics.Cms.Core.Enums.Admin;
using System.Collections.Generic;
using System.Web.Mvc;

namespace _6tactics.Cms.Core.ViewModels.Admin
{
    public class CreateEditDataViewModel
    {
        public int? ParentId { get; set; }
        public string ElementLanguage { get; set; }
        public string SelectedTitle { get; set; }
        public ContentType SelectedContentType { get; set; }
        public ContentType ParentContentType { get; set; }
        public ContentType CurrentContentType { get; set; }
        public string SelectedDisplayType { get; set; }
        public string ParentDisplayType { get; set; }
        public IEnumerable<SelectListItem> ContentTypeList { get; set; }
        public IEnumerable<SelectListItem> DisplayTypeList { get; set; }
        public IEnumerable<SelectListItem> ParentElementList { get; set; }
        public IEnumerable<SelectListItem> ElementWidthList { get; set; }
        public IEnumerable<SelectListItem> PageList { get; set; }
        public bool IsParentElementOrFile { get; set; }
        public bool? IsCreateChild { get; set; }
        //public int ScrollTopPosition { get; set; }
    }
}
