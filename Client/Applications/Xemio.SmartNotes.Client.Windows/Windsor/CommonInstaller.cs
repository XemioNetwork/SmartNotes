using Caliburn.Micro;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Xemio.CommonLibrary.Storage;
using Xemio.SmartNotes.Client.Abstractions.Interaction;
using Xemio.SmartNotes.Client.Abstractions.Settings;
using Xemio.SmartNotes.Client.Windows.Implementations.Interaction;
using Xemio.SmartNotes.Client.Windows.Implementations.Settings;

namespace Xemio.SmartNotes.Client.Windows.Windsor
{
    /// <summary>
    /// Installs all common components.
    /// </summary>
    public class CommonInstaller : IWindsorInstaller
    {
        #region Implementation of IWindsorInstaller
        /// <summary>
        /// Performs the installation in the <see cref="T:Castle.Windsor.IWindsorContainer"/>.
        /// </summary>
        /// <param name="container">The container.</param><param name="store">The configuration store.</param>
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register
            (
                Component.For<IWindowManager>().ImplementedBy<XemioWindowManager>().LifestyleSingleton(),
                Component.For<IEventAggregator>().ImplementedBy<EventAggregator>().LifestyleSingleton(),
                Component.For<IMessageManager>().ImplementedBy<MessageManager>().LifestyleSingleton(),
                Component.For<IDataStorage>().ImplementedBy<DataStorage>().LifestyleSingleton(),
                Component.For<ILanguageManager>().ImplementedBy<LanguageManager>().LifestyleSingleton()
            );
        }
        #endregion
    }
}
