using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Xemio.SmartNotes.Client.Abstractions.Clients;
using Xemio.SmartNotes.Client.Shared.WebService;
using Xemio.SmartNotes.Client.Windows.Data;

namespace Xemio.SmartNotes.Client.Windows.Windsor
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
                .FromAssemblyContaining<Session>()
                .BasedOn<IClient>()
                .WithServiceFromInterface(typeof(IClient))
                .LifestyleSingleton()
                .Configure(f => f.DependsOn(new { baseAddress = "http://localhost" })));

            container.Register
            (
                Component.For<Session>().LifestyleSingleton(),
                Component.For<WebServiceClient>().LifestyleSingleton()
            );
        }
        #endregion
    }
}
