using _6tactics.Cms.Core.Entities;
using _6tactics.Cms.Core.Enums.Admin;
using _6tactics.Cms.Core.Models.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace _6tactics.Cms.Core.ViewModels.Common
{
    public class ContentItemViewModel : IContentItem //, IValidatableObject
    {
        public int? Id { get; set; }
        public int? ParentId { get; set; }
        public ContentItem Parent { get; set; }
        public int Priority { get; set; }

        [Url, StringLength(400), Display(Name = "Link to")]
        public string LinkTo { get; set; }

        [Display(Name = "Link option")]
        public LinkOption? LinkOption { get; set; }

        [Display(Name = "Section title visibility")]
        public SectionTitleVisibility? SectionTitleVisibility { get; set; }

        [Display(Name = "Element visibility")]
        public ElementVisibility? ElementVisibility { get; set; }

        [StringLength(100), Required(ErrorMessage = "Required")]
        public string Title { get; set; }

        [Display(Name = "Route title")]
        public string RouteTitle { get; set; }

        [AllowHtml]
        public string Summary { get; set; }

        [AllowHtml]
        public string Content { get; set; }

        [StringLength(10)]
        public string Culture { get; set; }

        [StringLength(300), Display(Name = "File URL")]
        public string FileUrl { get; set; }

        [StringLength(40), Display(Name = "Element width")]
        public string ElementWidth { get; set; }

        [Required(ErrorMessage = "Required"), Display(Name = "Content type")]
        public ContentType ContentType { get; set; }

        [Display(Name = "Display type")]
        public string DisplayType { get; set; }

        public ICollection<ContentItem> ContentItems { get; set; }

        public ContentItemViewModel()
        {
            ContentItems = new List<ContentItem>();
        }


        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    if (ContentType == ContentType.Image && string.IsNullOrWhiteSpace(FileUrl))
        //    {
        //        yield return new ValidationResult("If image you must set FileUrl", new[] { "FileUrl" });
        //    }
        //}
    }
}