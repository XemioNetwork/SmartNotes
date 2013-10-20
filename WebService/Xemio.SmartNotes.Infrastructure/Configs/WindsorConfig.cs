using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http;
using Castle.Windsor;
using Castle.Windsor.Installer;
using WebApiContrib.IoC.CastleWindsor;

namespace Xemio.SmartNotes.Infrastructure.Configs
{
    /// <summary>
    /// Configures the windsor container.
    /// </summary>
    public static class WindsorConfig
    {
        /// <summary>
        /// Configures the specified configuration to use a <see cref="WindsorResolver"/>
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public static void Configure(HttpConfiguration configuration)
        {
            var container = new WindsorContainer();
            container.Install(FromAssembly.This());

            configuration.DependencyResolver = new WindsorResolver(container);
        }
    }
}
