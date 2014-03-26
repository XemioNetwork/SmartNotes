using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Xemio.SmartNotes.Client.Shared.Tasks;
using Xemio.SmartNotes.Client.Windows.Implementations.Tasks;

namespace Xemio.SmartNotes.Client.Windows.Windsor
{
    public class TaskInstaller : IWindsorInstaller
    {
        #region Implementation of IWindsorInstaller
        /// <summary>
        /// Performs the installation in the <see cref="T:Castle.Windsor.IWindsorContainer" />.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="store">The configuration store.</param>
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<ITaskExecutor>().ImplementedBy<TaskExecutor>().LifestyleSingleton());

            container.Register(Classes.FromThisAssembly()
                                      .BasedOn<ITask>()
                                      .LifestyleTransient());
        }
        #endregion
    }
}
