using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using SimpleInjector;
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

            this.Start();
        }
        #endregion

        #region Overrides of BootstrapperBase
        /// <summary>
        /// Override this to add custom behavior to execute after the application starts.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The args.</param>
        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            bool? loggedIn = this.ShowLoginWindow();

            if (loggedIn.HasValue && loggedIn.Value)
            {
                this.ShowShellWindow();
            }
            else
            {
                Application.Shutdown();
            }
        }
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

        #region Private Methods
        /// <summary>
        /// Shows the login window and returns the dialog result.
        /// </summary>
        private bool? ShowLoginWindow()
        {
            var loginViewModel = this._container.GetInstance<LoginViewModel>();

            IWindowManager windowManager = this._container.GetInstance<IWindowManager>();

            dynamic settings = new ExpandoObject();
            settings.ResizeMode = ResizeMode.CanMinimize;

            Application.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            bool? loggedIn = windowManager.ShowDialog(loginViewModel, null, settings);
            Application.ShutdownMode = ShutdownMode.OnLastWindowClose;

            return loggedIn;
        }
        /// <summary>
        /// Shows the shell window.
        /// </summary>
        private void ShowShellWindow()
        {
            IWindowManager windowManager = this._container.GetInstance<IWindowManager>();

            var shellViewModel = this._container.GetInstance<ShellViewModel>();
            windowManager.ShowWindow(shellViewModel);
        }
        #endregion
    }
}
