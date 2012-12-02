using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace PokerTable.Web
{
    /// <summary>
    /// Web API Config
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        /// Registers the specified config.
        /// </summary>
        /// <param name="config">The config.</param>
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });
        }
    }
}
