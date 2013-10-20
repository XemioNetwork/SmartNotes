using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Xemio.SmartNotes.Client.UserInterface.Common;

namespace Xemio.SmartNotes.Client.Windsor
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
                Component.For<IMessageManager>().ImplementedBy<MessageManager>().LifestyleSingleton()
            );
        }
        #endregion
    }
}
