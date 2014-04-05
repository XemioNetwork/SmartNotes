using Caliburn.Micro;
using Castle.Core;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Xemio.SmartNotes.Client.Shared.Clients;
using Xemio.SmartNotes.Client.Windows.Views.FacebookLogin;

namespace Xemio.SmartNotes.Client.Windows.Windsor
{
    /// <summary>
    /// Installs all view models.
    /// </summary>
    public class ViewModelInstaller : IWindsorInstaller
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
                .BasedOn<PropertyChangedBase>()
                .LifestyleTransient()
                .ConfigureFor<FacebookLoginViewModel>(f => f.DependsOn(
                    Dependency.OnValue<string>("547331422049204"), 
                    Dependency.OnComponent<WebServiceClient, WebServiceClient>())));
        }
        #endregion
    }
}
