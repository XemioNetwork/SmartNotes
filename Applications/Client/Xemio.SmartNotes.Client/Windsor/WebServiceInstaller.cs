using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Xemio.SmartNotes.Abstractions.Controllers;
using Xemio.SmartNotes.Client.Data;

namespace Xemio.SmartNotes.Client.Windsor
{
    /// <summary>
    /// Installs all webservices.
    /// </summary>
    public class WebServiceInstaller : IWindsorInstaller
    {
        #region Implementation of IWindsorInstaller
        /// <summary>
        /// Performs the installation in the <see cref="T:Castle.Windsor.IWindsorContainer"/>.
        /// </summary>
        /// <param name="container">The container.</param><param name="store">The configuration store.</param>
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes
                .FromThisAssembly()
                .BasedOn<IController>()
                .WithServiceFirstInterface()
                .LifestyleSingleton()
                .Configure(f => f.DependsOn(new { baseAddress = "http://localhost" })));

            container.Register(Component.For<Session>().LifestyleSingleton());
        }
        #endregion
    }
}
