using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace QuickStartProject.Web.UI.App_Start
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.LowercaseUrls = true;

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapHttpRoute(
                name: "ApiApplication",
                routeTemplate: "api/v1/acc/{acc}/app/{app}",
                defaults: new {controller = "Application"}
                );

            routes.MapHttpRoute(
                name: "ApiLog",
                routeTemplate: "api/v1/acc/{acc}/app/{app}/logs/{id}",
                defaults: new {controller = "Log", id = UrlParameter.Optional}
                );

            routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/v1/{controller}/{id}",
                defaults: new {id = RouteParameter.Optional}
                );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new {controller = "Home", action = "Index", id = UrlParameter.Optional}
                );
        }
    }
}