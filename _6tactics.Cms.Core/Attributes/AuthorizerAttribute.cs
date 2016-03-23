using System;
using System.Web.Mvc;

namespace _6tactics.Cms.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class AuthorizerAttribute : AuthorizeAttribute
    {
        // Set default Unauthorized Page Url here
        public string RedirectUrl { get; set; } = "/error/methodnotallowed";

        //private void CacheValidateHandler(HttpContext context, object data, ref HttpValidationStatus validationStatus)
        //{
        //    validationStatus = OnCacheAuthorization(new HttpContextWrapper(context));
        //}

        //private void SetCachePolicy(AuthorizationContext filterContext)
        //{
        //    HttpCachePolicyBase cache = filterContext.HttpContext.Response.Cache;
        //    cache.SetProxyMaxAge(new TimeSpan(0L));
        //    cache.AddValidationCallback(CacheValidateHandler, null);
        //}

        //public override void OnAuthorization(AuthorizationContext filterContext)
        //{
        //    Debug.WriteLine(filterContext.HttpContext.User.Identity.IsAuthenticated);

        //    if (filterContext == null)
        //        throw new ArgumentNullException("filterContext");

        //    if (OutputCacheAttribute.IsChildActionCacheActive(filterContext))
        //        throw new InvalidOperationException("CannotUseWithinChildActionCache");

        //    if (filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true) ||
        //        filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true))
        //        return;

        //    if (AuthorizeCore(filterContext.HttpContext))
        //        SetCachePolicy(filterContext);

        //    else if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
        //    {
        //        // auth failed, redirect to login page
        //        filterContext.Result = new HttpUnauthorizedResult();
        //    }
        //    else if (filterContext.HttpContext.User.IsInRole("SuperUser"))
        //    {
        //        // is authenticated and is in the SuperUser role
        //        SetCachePolicy(filterContext);
        //    }
        //    else
        //        HandleUnauthorizedRequest(filterContext);
        //}

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
                base.HandleUnauthorizedRequest(filterContext);
            else
                filterContext.Result = new RedirectResult(RedirectUrl);
        }
    }
}
