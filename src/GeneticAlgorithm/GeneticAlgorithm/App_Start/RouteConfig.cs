using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace GeneticAlgorithm
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Evolution",
                url: "evolution/{evolutionId}",
                defaults: new { controller = "Home", action = "Evolution", id = UrlParameter.Optional }
            );


            routes.MapRoute(
                name: "Generate",
                url: "{populationCount}/{valueCount}/{variableCount}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Evolve",
                url: "evolve",
                defaults: new { controller = "Home", action = "Evolve", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "EvolveToEnd",
                url: "evolvetoend",
                defaults: new { controller = "Home", action = "EvolveToEnd", id = UrlParameter.Optional }
            );
        }
    }
}
