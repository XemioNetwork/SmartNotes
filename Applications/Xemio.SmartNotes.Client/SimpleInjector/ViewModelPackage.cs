using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using SimpleInjector;
using SimpleInjector.Packaging;
using Xemio.SmartNotes.Client.UserInterface.Views.Login;

namespace Xemio.SmartNotes.Client.SimpleInjector
{
    /// <summary>
    /// Registers all viewmodels.
    /// </summary>
    public class ViewModelPackage : IPackage
    {
        #region Implementation of IPackage
        /// <summary>
        /// Registers the set of services in the specified <paramref name="container"/>.
        /// </summary>
        /// <param name="container">The container the set of services is registered into.</param>
        public void RegisterServices(Container container)
        {
            var viewModels = from type in typeof(ViewModelPackage).Assembly.GetExportedTypes()
                             where typeof(PropertyChangedBase).IsAssignableFrom(type)
                             select type;

            foreach (var viewModel in viewModels)
            {
                container.Register(viewModel);
            }
        }
        #endregion
    }
}
