using System.Web.Mvc;
using System.Web.Routing;

namespace _6tactics.Cms.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.LowercaseUrls = true;
            routes.MapMvcAttributeRoutes();
            //routes.AppendTrailingSlash = true;


            routes.MapRoute(
                 name: "Plugins",
                 url: "Plugins/{controller}/{action}/{id}"//,
                                                          //defaults:
                                                          //new { controller = "MailSender", action = "MailForm", pluginName = UrlParameter.Optional },
                                                          //namespaces: new[] { "_6tactics.Cms.Web.Areas.Plugins.Controllers" }
             ).DataTokens.Add("area", "Plugins");

            routes.MapRoute(
                name: "AdminCreate",
                url: "Admin/Create/{parentId}/{language}",
                defaults: new { controller = "Admin", action = "Create", parentId = UrlParameter.Optional, language = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "AdminDetails",
                url: "Admin/Details/{id}",
                defaults: new { controller = "Admin", action = "Details", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "AdminDelete",
                url: "Admin/Delete/{id}",
                defaults: new { controller = "Admin", action = "Delete", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "AdminEdit",
                url: "Admin/Edit/{id}/{language}",
                defaults: new { controller = "Admin", action = "Edit", id = UrlParameter.Optional, language = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "AdminDefault",
                url: "Admin/{action}/{lang}/{pageId}",
                defaults: new { controller = "Admin", action = "Index", lang = UrlParameter.Optional, pageId = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "AccountDefault",
                url: "Account/{action}/{id}",
                defaults:
                    new { controller = "Account", action = "Login", id = UrlParameter.Optional }//,
                    //namespaces: new[] { "_6tactics.Cms.Web.Controllers" }
                );

            routes.MapRoute(
                name: "AjaxDefault",
                url: "Ajax/{action}/{id}",
                defaults:
                    new { controller = "Ajax", action = UrlParameter.Optional, id = UrlParameter.Optional }//,
                    //namespaces: new[] { "_6tactics.Cms.Web.Controllers" }
                );

            routes.MapRoute(
                name: "Popup",
                url: "Popup/{action}/{id}/",
                defaults:
                new { controller = "Popup" }//,
                //namespaces: new[] { "_6tactics.Cms.Web.Controllers" }
            );

            routes.MapRoute(
                name: "GetFiles",
                url: "Web/GetFile/{folderPath}/{filename}/{extensionName}",
                defaults:
                new { controller = "Web", action = "GetFile" }//,
                //namespaces: new[] { "_6tactics.Cms.Web.Controllers" }
            );

            routes.MapRoute(
                name: "Web",
                url: "{controller}/{action}/{lang}/{routeTitle}/",
                defaults:
                new { controller = "Web", action = "Index", lang = UrlParameter.Optional, routeTitle = UrlParameter.Optional }//,
                //namespaces: new[] { "_6tactics.Cms.Web.Controllers" }
            );

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{lang}/{pageId}/",
            //    defaults:
            //    new { controller = "Web", action = "Index", lang = UrlParameter.Optional, pageId = UrlParameter.Optional },
            //    namespaces: new[] { "AreaControllersNamespace" }
            //).DataTokens.Add("area", "AreaName");

        }
    }
}
