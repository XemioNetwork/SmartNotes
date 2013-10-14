using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using SimpleInjector;
using SimpleInjector.Packaging;

namespace Xemio.SmartNotes.Client.SimpleInjector
{
    /// <summary>
    /// Registers caliburn classes.
    /// </summary>
    public class CaliburnPackage : IPackage
    {
        #region Implementation of IPackage
        /// <summary>
        /// Registers the set of services in the specified <paramref name="container"/>.
        /// </summary>
        /// <param name="container">The container the set of services is registered into.</param>
        public void RegisterServices(Container container)
        {
            container.RegisterSingle<IWindowManager, WindowManager>();
            container.RegisterSingle<IEventAggregator, EventAggregator>();
        }
        #endregion
    }
}
