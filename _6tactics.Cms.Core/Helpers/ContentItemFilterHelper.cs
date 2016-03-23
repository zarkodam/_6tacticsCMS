using _6tactics.Cms.Core.Entities;
using _6tactics.Cms.Core.Enums.Admin;
using System;
using System.Linq.Expressions;

namespace _6tactics.Cms.Core.Helpers
{
    public class ContentItemFilterHelper
    {
        public Expression<Func<ContentItem, bool>> IsProject
        {
            get { return (i) => i.ContentType == ContentType.Project; }
        }

        public Expression<Func<ContentItem, bool>> IsLanguage(string language)
        {
            return (i) => i.ContentType == ContentType.Language && i.Title == language;
        }

        public Expression<Func<ContentItem, bool>> IsPage(int? pageId)
        {
            return (i) => i.ContentType == ContentType.Page && i.Id == pageId;
        }
    }
}
