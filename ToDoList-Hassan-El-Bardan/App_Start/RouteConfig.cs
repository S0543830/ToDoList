using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ToDoList_Hassan_El_Bardan
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
              name: "deleteRoh",
              url: "{controller}/{action}/{Id}/{roh}",
              defaults: new { controller = "Home", action = "DeleteKategorie", id = UrlParameter.Optional }
          );
            routes.MapRoute(
                name: "addProdukt",
                url: "{controller}/{action}/{typ}",
                defaults: new { controller = "Home", action = "Create", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "AddProduktTyp",
                url: "{controller}/{action}/{_typ}",
                defaults: new { controller = "Home", action = "DeleteKategorie", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "DeleteKategorie",
                url: "{controller}/{action}/{_name}",
                defaults: new { controller = "Home", action = "DeleteKatekorie", id = UrlParameter.Optional }
            );
        }
    }
}
