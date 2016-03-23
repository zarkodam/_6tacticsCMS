using System.Web.Mvc;
using System.Web.Routing;

namespace _6tactics.Cms.Core.Attributes
{
    public class RemoveDuplicateContentAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            RouteCollection routes = RouteTable.Routes;

            RequestContext requestContext = filterContext.RequestContext;
            RouteData routeData = requestContext.RouteData;
            RouteValueDictionary dataTokens = routeData.DataTokens;

            if (dataTokens["area"] == null) dataTokens.Add("area", "");

            VirtualPathData vpd = routes.GetVirtualPathForArea(requestContext, routeData.Values);

            if (vpd != null)
            {
                var virtualPath = vpd.VirtualPath.ToLower();
                var request = requestContext.HttpContext.Request;

                if (!string.Equals(virtualPath, request.Path))
                    filterContext.Result = new RedirectResult(virtualPath + request.Url?.Query, true);
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
