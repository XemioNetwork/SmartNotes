using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Caliburn.Micro;
using Castle.Core.Logging;
using Castle.Facilities.Logging;
using Castle.Windsor;
using Castle.Windsor.Installer;
using CefSharp;
using Xemio.SmartNotes.Client.Windows.Themes.Controls;
using Xemio.SmartNotes.Client.Windows.Views.Login;
using Xemio.SmartNotes.Client.Windows.Views.Shell;

namespace Xemio.SmartNotes.Client.Windows
{
    /// <summary>
    /// A bootstrapper configuring the whole application, showing the login window and then the shell.
    /// </summary>
    public class Bootstrapper : BootstrapperBase
    {
        #region Fields
        private WindsorContainer _container;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Bootstrapper"/> class.
        /// </summary>
        public Bootstrapper()
        {
            this.Initialize();
        }
        #endregion

        #region Overrides of BootstrapperBase
        /// <summary>
        /// Override to configure the framework and setup your IoC container.
        /// </summary>
        protected override void Configure()
        {
            this._container = new WindsorContainer();
            this._container.Install(FromAssembly.This());

            this.InitializeConventions();
            this.InitializeCef();
        }
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
        /// Called when the application starts.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="StartupEventArgs"/> instance containing the event data.</param>
        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            Application.ShutdownMode = ShutdownMode.OnExplicitShutdown;

            bool loggedOut = true;

            //While we log us out from the shell-view we show the login view again
            while (loggedOut)
            {
                //We logged us out if we successfully logged us in and then logged us out in the shell
                loggedOut = this.ShowLoginView() && this.ShowShellView();
            }

            Application.Shutdown();
        }
        /// <summary>
        /// Called when the application shuts down.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        protected override void OnExit(object sender, EventArgs e)
        {
            this._container.Dispose();
        }
        /// <summary>
        /// Override this to add custom behavior for unhandled exceptions.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        protected override void OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            var loggerFactory = this._container.Resolve<ILoggerFactory>();

            ILogger logger = loggerFactory.Create("Default");
            logger.Error("An unhandled error occured.", e.Exception);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Shows the login view.
        /// </summary>
        private bool ShowLoginView()
        {
            var windowManager = this._container.Resolve<IWindowManager>();

            dynamic settings = new ExpandoObject();
            settings.ResizeMode = ResizeMode.CanMinimize;

            var loginViewModel = this._container.Resolve<LoginViewModel>();
            bool? loggedIn = windowManager.ShowDialog(loginViewModel, null, settings);
            this._container.Release(loginViewModel);

            return loggedIn.HasValue && loggedIn.Value;
        }
        /// <summary>
        /// Shows the shell with the given user user.
        /// </summary>
        private bool ShowShellView()
        {
            var windowManager = this._container.Resolve<IWindowManager>();

            dynamic settings = new ExpandoObject();
            settings.MinWidth = 800;
            settings.MinHeight = 600;
            settings.Width = 800;
            settings.Height = 600;
            settings.SizeToContent = SizeToContent.Manual;

            var shellViewModel = this._container.Resolve<ShellViewModel>();
            bool? loggedOut = windowManager.ShowDialog(shellViewModel, null, settings);
            this._container.Release(shellViewModel);

            return loggedOut.HasValue && loggedOut.Value;
        }
        /// <summary>
        /// Initializes the conventions.
        /// </summary>
        private void InitializeConventions()
        {
            ConventionManager.AddElementConvention<WatermarkPasswordBox>(WatermarkPasswordBox.PasswordProperty, "Password", "PasswordChanged");
        }
        /// <summary>
        /// Initializes the chromium embedded framework.
        /// </summary>
        private void InitializeCef()
        {
            var settings = new Settings
            {
                AutoDetectProxySettings = true,
                UserAgent = "Xemio Notes Desktop Client",
                PackLoadingDisabled = true
            };

            if (CEF.Initialize(settings) == false)
            {
                throw new ApplicationException("Could not initialize 'CEF'.");
            }
        }
        #endregion
    }
}
