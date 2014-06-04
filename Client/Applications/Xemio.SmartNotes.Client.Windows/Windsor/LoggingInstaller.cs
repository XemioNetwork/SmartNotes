using System.Windows.Navigation;
using Caliburn.Micro;
using Castle.Facilities.Logging;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Xemio.CommonLibrary.Security;
using Xemio.CommonLibrary.Storage;
using Xemio.CommonLibrary.Storage.Files;
using Xemio.SmartNotes.Client.Shared.Interaction;
using Xemio.SmartNotes.Client.Shared.Settings;
using Xemio.SmartNotes.Client.Shared.Tasks;
using Xemio.SmartNotes.Client.Windows.Implementations.Interaction;
using Xemio.SmartNotes.Client.Windows.Implementations.Settings;
using Xemio.SmartNotes.Client.Windows.Implementations.Tasks;

namespace Xemio.SmartNotes.Client.Windows.Windsor
{
    public class LoggingInstaller : IWindsorInstaller
    {
        #region Implementation of IWindsorInstaller
        /// <summary>
        /// Performs the installation in the <see cref="T:Castle.Windsor.IWindsorContainer"/>.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="store">The configuration store.</param>
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.AddFacility<LoggingFacility>(f => f.UseNLog().WithAppConfig());
        }
        #endregion
    }
}
