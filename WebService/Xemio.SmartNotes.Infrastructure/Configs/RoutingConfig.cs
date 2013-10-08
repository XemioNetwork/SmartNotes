using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http;
using AttributeRouting.Web.Http.SelfHost;
using Xemio.SmartNotes.Infrastructure.Controllers;

namespace Xemio.SmartNotes.Infrastructure.Configs
{
    /// <summary>
    /// Configures the request routing.
    /// </summary>
    public static class RoutingConfig
    {
        /// <summary>
        /// Configures the specified configuration to use attribute routing.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public static void Configure(HttpConfiguration configuration)
        {
            configuration.Routes.MapHttpAttributeRoutes(f => f.AddRoutesFromAssemblyOf<BaseController>());
        }
    }
}
