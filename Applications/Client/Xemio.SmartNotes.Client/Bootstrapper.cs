using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using SimpleInjector;
using Xemio.SmartNotes.Client.Views.Shell;

namespace Xemio.SmartNotes.Client
{
    /// <summary>
    /// A bootstrapper configuring the whole application, showing the login window and then the shell.
    /// </summary>
    public class Bootstrapper : Bootstrapper<ShellViewModel>
    {
        #region Fields
        private readonly Container _container;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Bootstrapper"/> class.
        /// </summary>
        public Bootstrapper()
        {
            this._container = new Container();
            this._container.RegisterPackages();
        }
        #endregion

        #region Overrides of Bootstrapper
        /// <summary>
        /// Override this to provide an IoC specific implementation.
        /// </summary>
        /// <param name="service">The service to locate.</param>
        /// <param name="key">The key to locate.</param>
        protected override object GetInstance(Type service, string key)
        {
            return this._container.GetInstance(service);
        }
        /// <summary>
        /// Override this to provide an IoC specific implementation
        /// </summary>
        /// <param name="service">The service to locate.</param>
        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return this._container.GetAllInstances(service);
        }
        /// <summary>
        /// Override this to provide an IoC specific implementation.
        /// </summary>
        /// <param name="instance">The instance to perform injection on.</param>
        protected override void BuildUp(object instance)
        {
            this._container.InjectProperties(instance);
        }
        #endregion
    }
}
