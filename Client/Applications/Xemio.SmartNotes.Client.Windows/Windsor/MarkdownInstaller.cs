using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Xemio.SmartNotes.Client.Shared.Interaction;
using Xemio.SmartNotes.Client.Windows.Implementations.Interaction;

namespace Xemio.SmartNotes.Client.Windows.Windsor
{
    public class MarkdownInstaller : IWindsorInstaller
    {
        #region Implementation of IWindsorInstaller
        /// <summary>
        /// Performs the installation in the <see cref="T:Castle.Windsor.IWindsorContainer" />.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="store">The configuration store.</param>
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IMarkdownConverter>().ImplementedBy<MarkdownConverter>().LifestyleSingleton());
        }
        #endregion
    }
}
