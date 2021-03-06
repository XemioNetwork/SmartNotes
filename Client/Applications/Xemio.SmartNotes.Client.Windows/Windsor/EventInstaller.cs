﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Xemio.SmartNotes.Client.Windows.Windsor
{
    public class EventInstaller : IWindsorInstaller
    {
        #region Implementation of IWindsorInstaller
        /// <summary>
        /// Performs the installation in the <see cref="T:Castle.Windsor.IWindsorContainer" />.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="store">The configuration store.</param>
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IEventAggregator>().ImplementedBy<EventAggregator>().LifestyleSingleton());
        }
        #endregion
    }
}
