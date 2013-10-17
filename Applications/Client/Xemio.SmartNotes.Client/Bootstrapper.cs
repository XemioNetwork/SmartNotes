using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Xemio.SmartNotes.Client.UserInterface.Views.Login;
using Xemio.SmartNotes.Client.UserInterface.Views.Shell;

namespace Xemio.SmartNotes.Client
{
    /// <summary>
    /// A bootstrapper configuring the whole application, showing the login window and then the shell.
    /// </summary>
    public class Bootstrapper : BootstrapperBase
    {
        #region Fields
        private readonly WindsorContainer _container;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Bootstrapper"/> class.
        /// </summary>
        public Bootstrapper()
        {
            this._container = new WindsorContainer();
            this._container.Install(FromAssembly.This());

            this.Start();
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
            if (this._container.Kernel.HasComponent(service) == false)
                return base.GetInstance(service, key);

            return this._container.Resolve(service);
        }
        /// <summary>
        /// Override this to provide an IoC specific implementation
        /// </summary>
        /// <param name="service">The service to locate.</param>
        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            if (this._container.Kernel.HasComponent(service) == false)
                return base.GetAllInstances(service);

            return this._container.ResolveAll(service).OfType<object>();
        }
        /// <summary>
        /// Override this to provide an IoC specific implementation.
        /// </summary>
        /// <param name="instance">The instance to perform injection on.</param>
        protected override void BuildUp(object instance)
        {
            IEnumerable<PropertyInfo> propertiesToInject = instance
                .GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(f => f.CanWrite && this._container.Kernel.HasComponent(f.PropertyType));

            foreach (var property in propertiesToInject)
            {
                property.SetValue(instance, this._container.Resolve(property.PropertyType));
            }
        }
        /// <summary>
        /// Called when [startup].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="StartupEventArgs"/> instance containing the event data.</param>
        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            bool? loggedIn = this.ShowLoginView();

            if (loggedIn.HasValue && loggedIn.Value)
            {
                this.ShowShellView();
            }
            else
            {
                Application.Shutdown();
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Shows the login view.
        /// </summary>
        private bool? ShowLoginView()
        {
            var loginViewModel = this._container.Resolve<LoginViewModel>();

            IWindowManager windowManager = this._container.Resolve<IWindowManager>();

            dynamic settings = new ExpandoObject();
            settings.ResizeMode = ResizeMode.CanMinimize;

            Application.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            bool? loggedIn = windowManager.ShowDialog(loginViewModel, null, settings);
            Application.ShutdownMode = ShutdownMode.OnLastWindowClose;

            return loggedIn;
        }
        /// <summary>
        /// Shows the shell with the given user user.
        /// </summary>
        private void ShowShellView()
        {
            IWindowManager windowManager = this._container.Resolve<IWindowManager>();

            var shellViewModel = this._container.Resolve<ShellViewModel>();
            windowManager.ShowWindow(shellViewModel);
        }
        #endregion
    }
}
