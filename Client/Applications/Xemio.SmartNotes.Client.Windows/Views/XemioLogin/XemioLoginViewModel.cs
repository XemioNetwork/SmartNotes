using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Windows;
using Caliburn.Micro;
using Xemio.CommonLibrary.Storage;
using Xemio.SmartNotes.Client.Shared.Clients;
using Xemio.SmartNotes.Client.Shared.Settings;
using Xemio.SmartNotes.Client.Windows.Implementations.Interaction;
using Xemio.SmartNotes.Client.Windows.Views.Login;
using Xemio.SmartNotes.Client.Windows.Views.PasswordReset;
using Xemio.SmartNotes.Client.Windows.Views.Register;
using Xemio.SmartNotes.Shared.Entities.Users;
using Xemio.SmartNotes.Shared.Models;

namespace Xemio.SmartNotes.Client.Windows.Views.XemioLogin
{
    public class XemioLoginViewModel : Screen
    {
        #region Constants
        private const string UsernameKey = "EmailAddress";
        private const string PasswordKey = "Password";
        private const string RememberMeKey = "RememberMe";
        #endregion

        #region Fields
        private readonly WebServiceClient _webServiceClient;
        private readonly DisplayManager _displayManager;
        private readonly IDataStorage _dataStorage;

        private string _emailAddress;
        private string _password;
        private bool _rememberMe;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="LoginViewModel" /> class.
        /// </summary>
        /// <param name="webServiceClient">The webservice client.</param>
        /// <param name="displayManager">The display manager.</param>
        /// <param name="dataStorage">The data storage.</param>
        public XemioLoginViewModel(WebServiceClient webServiceClient, DisplayManager displayManager, IDataStorage dataStorage)
        {
            this.DisplayName = "Xemio Notes";

            this._webServiceClient = webServiceClient;
            this._displayManager = displayManager;
            this._dataStorage = dataStorage;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string EmailAddress
        {
            get { return this._emailAddress; }
            set
            {
                if (this._emailAddress != value)
                {
                    this._emailAddress = value;
                    this.NotifyOfPropertyChange(() => this.EmailAddress);
                    this.NotifyOfPropertyChange(() => this.CanLogin);
                }
            }
        }
        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        public string Password
        {
            get { return this._password; }
            set
            {
                if (this._password != value)
                {
                    this._password = value;
                    this.NotifyOfPropertyChange(() => this.Password);
                    this.NotifyOfPropertyChange(() => this.CanLogin);
                }
            }
        }
        /// <summary>
        /// Gets or sets a value indicating whether to remember the login credentials.
        /// </summary>
        public bool RememberMe
        {
            get { return this._rememberMe; }
            set
            {
                if (this._rememberMe != value)
                {
                    this._rememberMe = value;
                    this.NotifyOfPropertyChange(() => this.RememberMe);
                }
            }
        }
        /// <summary>
        /// Gets the authentication token.
        /// </summary>
        public AuthenticationToken Token { get; private set; }
        #endregion

        #region Overrides of Screen
        /// <summary>
        /// Called when initializing.
        /// </summary>
        protected override void OnInitialize()
        {
            this.LoadRememberMe();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Gets a value indicating whether the <see cref="Login"/> method can be executed.
        /// </summary>
        public bool CanLogin
        {
            get
            {
                return string.IsNullOrEmpty(this.EmailAddress) == false &&
                       string.IsNullOrEmpty(this.Password) == false;
            }
        }
        /// <summary>
        /// Tries to login with the xemio credentials.
        /// </summary>
        public async Task Login()
        {
            HttpResponseMessage tokenResponse = await this._webServiceClient.Tokens.PostXemio(this.EmailAddress, this.Password);
            if (tokenResponse.StatusCode == HttpStatusCode.OK)
            {
                this.Token = await tokenResponse.Content.ReadAsAsync<AuthenticationToken>();
                this.SaveRememberMe();

                this.TryClose(true);
            }
            else if (tokenResponse.StatusCode == HttpStatusCode.Unauthorized)
            {
                var error = await tokenResponse.Content.ReadAsAsync<HttpError>();
                this._displayManager.Messages.ShowMessageBox(error.Message, XemioLoginMessages.LoginFailed, MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                var error = await tokenResponse.Content.ReadAsAsync<HttpError>();
                this._displayManager.Messages.ShowMessageBox(error.Message, XemioLoginMessages.UnknownError, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        } 
        /// <summary>
        /// Opens the register window.
        /// </summary>
        public void Register()
        {
            var registerViewModel = IoC.Get<RegisterViewModel>();

            dynamic settings = new ExpandoObject();
            settings.ResizeMode = ResizeMode.CanMinimize;

            this._displayManager.Windows.ShowDialog(registerViewModel, null, settings);
        }
        /// <summary>
        /// Opens the forgot-password window.
        /// </summary>
        public void ForgotPassword()
        {
            var passwordResetViewModel = IoC.Get<PasswordResetViewModel>();

            dynamic settings = new ExpandoObject();
            settings.ResizeMode = ResizeMode.CanMinimize;

            this._displayManager.Windows.ShowDialog(passwordResetViewModel, null, settings);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Tries to load the local settings.
        /// </summary>
        private void LoadRememberMe()
        {
            if (this._dataStorage.Retrieve<bool>(RememberMeKey))
            {
                this.RememberMe = true;
                this.EmailAddress = this._dataStorage.Retrieve<string>(UsernameKey);
                this.Password = this._dataStorage.Retrieve<string>(PasswordKey);
            }
        }
        /// <summary>
        /// Saves the local settings.
        /// </summary>
        private void SaveRememberMe()
        {
            this._dataStorage.Store(this.RememberMe, RememberMeKey);
            this._dataStorage.Store(this.EmailAddress, UsernameKey);
            this._dataStorage.Store(this.Password, PasswordKey);
        }
        #endregion
    }
}
