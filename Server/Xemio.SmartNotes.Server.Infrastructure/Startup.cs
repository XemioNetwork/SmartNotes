using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using Owin;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Xemio.SmartNotes.Server.Infrastructure.Filters;
using Xemio.SmartNotes.Server.Infrastructure.Windsor;

namespace Xemio.SmartNotes.Server.Infrastructure
{
    /// <summary>
    /// Configures the SmartNotes webservice.
    /// </summary>
    public class Startup
    {
        #region Methods
        /// <summary>
        /// Configurations the webservices.
        /// </summary>
        /// <param name="appBuilder">The app builder.</param>
        public void Configuration(IAppBuilder appBuilder)
        {
            var config = new HttpConfiguration();

            config.EnableCors(new EnableCorsAttribute("*", "*", "*"));

            this.ConfigureWindsor(config);
            this.ConfigureFilters(config);
            this.ConfigureRoutes(config);

            appBuilder.UseWebApi(config);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Configures the castle windsor IoC container.
        /// </summary>
        /// <param name="config">The config.</param>
        private void ConfigureWindsor(HttpConfiguration config)
        {
            var container = new WindsorContainer();
            container.Install(FromAssembly.This());

            config.DependencyResolver = new WindsorResolver(container);
        }
        /// <summary>
        /// Configures the filters.
        /// </summary>
        /// <param name="config">The config.</param>
        private void ConfigureFilters(HttpConfiguration config)
        {
            config.Filters.Add(new HandleBusinessExceptionAttribute());
        }
        /// <summary>
        /// Configures the routes.
        /// </summary>
        /// <param name="config">The config.</param>
        private void ConfigureRoutes(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();
        }
        #endregion
    }
}
