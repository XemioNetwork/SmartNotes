using System;
using System.Net;
using System.Net.Http;
using System.Windows;
using Caliburn.Micro;
using Xemio.CommonLibrary.Storage;
using Xemio.SmartNotes.Abstractions.Controllers;
using Xemio.SmartNotes.Client.Abstractions.Interaction;
using Xemio.SmartNotes.Client.Abstractions.Settings;
using Xemio.SmartNotes.Client.Shared.WebService;
using Xemio.SmartNotes.Client.Windows.Data;
using Xemio.SmartNotes.Client.Windows.Views.Login.Resources;
using Xemio.SmartNotes.Models.Entities.Users;

namespace Xemio.SmartNotes.Client.Windows.Views.Login
{
    public class LoginViewModel : Screen
    {
        #region Constants
        private const string UsernameKey = "Username";
        private const string PasswordKey = "Password";
        private const string RememberMeKey = "RememberMe";
        #endregion

        #region Fields
        private readonly IUsersController _usersController;
        private readonly IWindowManager _windowManager;
        private readonly IMessageManager _messageManager;
        private readonly ILanguageManager _languageManager;
        private readonly IDataStorage _dataStorage;

        private readonly Session _session;

        private string _username;
        private string _password;
        private bool _rememberMe;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Username
        {
            get { return this._username; }
            set
            {
                if (this._username != value)
                {
                    this._username = value;
                    this.NotifyOfPropertyChange(() => this.Username);
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
        /// Gets or sets a value indicating whether [remember me].
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
        /// Gets a value indicating whether the <see cref="Login"/> method can be executed.
        /// </summary>
        public bool CanLogin
        {
            get
            {
                return string.IsNullOrWhiteSpace(this.Username) == false &&
                       string.IsNullOrWhiteSpace(this.Password) == false;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="LoginViewModel" /> class.
        /// </summary>
        /// <param name="usersController">The users controller.</param>
        /// <param name="windowManager">The window manager.</param>
        /// <param name="messageManager">The message manager.</param>
        /// <param name="languageManager">The language manager.</param>
        /// <param name="dataStorage">The data storage.</param>
        /// <param name="session">The application session.</param>
        public LoginViewModel(IUsersController usersController, IWindowManager windowManager, IMessageManager messageManager, ILanguageManager languageManager, IDataStorage dataStorage, Session session)
        {
            this.DisplayName = "Xemio Notes";

            this._usersController = usersController;
            this._windowManager = windowManager;
            this._messageManager = messageManager;
            this._languageManager = languageManager;
            this._dataStorage = dataStorage;

            this._session = session;
        }
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
        /// Tries to login with the current credentials.
        /// </summary>
        public async void Login()
        {
            this._session.Username = this.Username;
            this._session.Password = this.Password;

            HttpResponseMessage response = await this._usersController.GetCurrent();
            if (response.StatusCode == HttpStatusCode.Found)
            {
                this.SaveRememberMe();

                this._session.User = await response.Content.ReadAsAsync<User>();
                this.TryClose(true);
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                string message = await response.Content.ReadAsStringAsync();
                this._messageManager.ShowMessageBox(message, LoginMessages.LoginFailed, MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        /// <summary>
        /// Opens the register window.
        /// </summary>
        public void Register()
        {
        }
        /// <summary>
        /// Opens the forgot-password window.
        /// </summary>
        public void ForgotPassword()
        {
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
                this.Username = this._dataStorage.Retrieve<string>(UsernameKey);
                this.Password = this._dataStorage.Retrieve<string>(PasswordKey);
            }
        }
        /// <summary>
        /// Saves the local settings.
        /// </summary>
        private void SaveRememberMe()
        {
            this._dataStorage.Store(this.RememberMe, RememberMeKey);
            this._dataStorage.Store(this.Username, UsernameKey);
            this._dataStorage.Store(this.Password, PasswordKey);
        }
        #endregion
    }
}
