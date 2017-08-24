using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace MIMWebClient
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            routes.MapHttpRoute(
                name: "API Default",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
);

            routes.MapRoute(
                name: "Room",
                url: "{controller}/{action}/{region}/{area}/{areaId}",
                defaults: new { controller = "Home", action = "Index", region = UrlParameter.Optional, area = UrlParameter.Optional, areaId = UrlParameter.Optional }
            );


            routes.MapRoute(
                  "Default",
                 "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new[] { "MIMWebClient.Controllers" }
            );
        }
    }
}
