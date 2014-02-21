using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Facilities.QuartzIntegration;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Quartz;
using Quartz.Impl;

namespace Xemio.SmartNotes.Server.Infrastructure.Windsor
{
    /// <summary>
    /// Installs the Quartz.NET scheduler classes.
    /// </summary>
    public class QuartzNetInstaller : IWindsorInstaller
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
                    Component.For<IScheduler>()
                        .Instance(this.GetScheduler(container))
                        .LifestyleSingleton(),
                    Classes.FromThisAssembly()
                        .BasedOn<IJob>()
                        .WithServiceSelf()
                        .LifestyleTransient()
                );
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Configures and returns the <see cref="IScheduler"/>.
        /// </summary>
        /// <param name="container">The container.</param>
        private IScheduler GetScheduler(IWindsorContainer container)
        {
            var scheduler = StdSchedulerFactory.GetDefaultScheduler();

            scheduler.JobFactory = new WindsorJobFactory(container.Kernel);

            return scheduler;
        }
        #endregion
    }
}
