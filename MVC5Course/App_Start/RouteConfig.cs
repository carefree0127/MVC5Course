using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MVC5Course
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");//Web From有很多檔案都是axd結尾

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                //url: "{action}/{controller}/{id}",
                //可自訂URL，可把關鍵字.Metadata加在網址增加SEO
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
