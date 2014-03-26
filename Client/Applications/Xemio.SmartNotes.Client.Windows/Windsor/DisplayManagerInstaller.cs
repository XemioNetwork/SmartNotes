using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Xemio.SmartNotes.Client.Shared.Interaction;
using Xemio.SmartNotes.Client.Shared.Settings;
using Xemio.SmartNotes.Client.Windows.Implementations.Interaction;
using Xemio.SmartNotes.Client.Windows.Implementations.Settings;

namespace Xemio.SmartNotes.Client.Windows.Windsor
{
    public class DisplayManagerInstaller : IWindsorInstaller
    {
        #region Implementation of IWindsorInstaller
        /// <summary>
        /// Performs the installation in the <see cref="T:Castle.Windsor.IWindsorContainer" />.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="store">The configuration store.</param>
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register
            (
                Component.For<DisplayManager>().LifestyleSingleton(),
                Component.For<IWindowManager>().ImplementedBy<XemioWindowManager>().LifestyleSingleton(),
                Component.For<IMessageManager>().ImplementedBy<MessageManager>().LifestyleSingleton(),
                Component.For<ILanguageManager>().ImplementedBy<LanguageManager>().LifestyleSingleton()
            );
        }
        #endregion
    }
}
