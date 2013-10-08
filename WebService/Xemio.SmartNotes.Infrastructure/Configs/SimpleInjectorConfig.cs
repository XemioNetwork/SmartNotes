using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http;
using SimpleInjector;
using SimpleInjector.Packaging;
using Xemio.SmartNotes.Infrastructure.SimpleInjector;
using Xemio.SmartNotes.Infrastructure.SimpleInjector.Extensions;

namespace Xemio.SmartNotes.Infrastructure.Configs
{
    /// <summary>
    /// Configures the windsor container.
    /// </summary>
    public static class SimpleInjectorConfig
    {
        /// <summary>
        /// Configures the specified configuration to use a <see cref="SimpleInjectorDependencyResolver"/>
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public static void Configure(HttpConfiguration configuration)
        {
            var container = new Container();
            container.RegisterPackages();

            configuration.DependencyResolver = new SimpleInjectorDependencyResolver(container);
        }
    }
}
