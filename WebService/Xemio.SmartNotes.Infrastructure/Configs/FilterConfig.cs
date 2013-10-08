using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Xemio.SmartNotes.Infrastructure.Filters;

namespace Xemio.SmartNotes.Infrastructure.Configs
{
    /// <summary>
    /// Configures the filters.
    /// </summary>
    public static class FilterConfig
    {
        /// <summary>
        /// Configures the specified configuration to use attribute routing.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public static void Configure(HttpConfiguration configuration)
        {
            configuration.Filters.Add(new BusinessExceptionFilterAttribute());
        }
    }
}
