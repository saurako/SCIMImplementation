using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SCIMAPI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "getAllUsers",
                url: "users/",
                defaults: new { controller = "Users", action = "GetAllUsers" }
            );

            routes.MapRoute(
                name: "getOneUser",
                url: "users/{id}",
                defaults: new { controller = "Users", action = "GetUserById", id = UrlParameter.Optional }
            );



            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}