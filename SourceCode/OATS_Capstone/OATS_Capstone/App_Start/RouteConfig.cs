using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace OATS_Capstone
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{subdomain}/{id}",
                defaults: new { controller = "Tests", action = "Index",subdomain=UrlParameter.Optional, id = UrlParameter.Optional }
            );
        }
    }
}