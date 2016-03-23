using System;
using System.Web;
using System.Web.Mvc;

namespace _6tactics.Cms.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public sealed class ChildActionOnlyCustomAttribute : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }
            if (!filterContext.IsChildAction)
            {
                throw new HttpException(404, "NOT FOUD");
            }
        }
    }
}
