﻿using System;
using System.Dynamic;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Windows;
using System.Windows.Navigation;
using Caliburn.Micro;
using Xemio.CommonLibrary.Storage;
using Xemio.SmartNotes.Client.Abstractions.Interaction;
using Xemio.SmartNotes.Client.Abstractions.Settings;
using Xemio.SmartNotes.Client.Shared.WebService;
using Xemio.SmartNotes.Client.Windows.Data;
using Xemio.SmartNotes.Client.Windows.Implementations.Interaction;
using Xemio.SmartNotes.Client.Windows.Views.PasswordReset;
using Xemio.SmartNotes.Client.Windows.Views.Register;
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
        private readonly WebServiceClient _webServiceClient;
        private readonly DisplayManager _displayManager;
        private readonly IDataStorage _dataStorage;
        private readonly ILanguageManager _languageManager;

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
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginViewModel" /> class.
        /// </summary>
        /// <param name="webServiceClient">The webservice client.</param>
        /// <param name="displayManager">The display manager.</param>
        /// <param name="dataStorage">The data storage.</param>
        /// <param name="languageManager">The language manager.</param>
        public LoginViewModel(WebServiceClient webServiceClient, DisplayManager displayManager, IDataStorage dataStorage, ILanguageManager languageManager)
        {
            this.DisplayName = "Xemio Notes";

            this._webServiceClient = webServiceClient;
            this._displayManager = displayManager;
            this._dataStorage = dataStorage;
            this._languageManager = languageManager;
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
        /// Gets a value indicating whether the <see cref="Login"/> method can be executed.
        /// </summary>
        public bool CanLogin
        {
            get
            {
                return string.IsNullOrEmpty(this.Username) == false &&
                       string.IsNullOrEmpty(this.Password) == false;
            }
        }
        /// <summary>
        /// Tries to login with the current credentials.
        /// </summary>
        public async void Login()
        {
            this._webServiceClient.Session.Username = this.Username;
            this._webServiceClient.Session.Password = this.Password;

            HttpResponseMessage response = await this._webServiceClient.Users.GetAuthorized();
            if (response.StatusCode == HttpStatusCode.Found)
            {
                this.SaveRememberMe();

                this._webServiceClient.Session.User = await response.Content.ReadAsAsync<User>();
                this.TryClose(true);
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                string message = await response.Content.ReadAsStringAsync();
                this._displayManager.Messages.ShowMessageBox(message, LoginMessages.LoginFailed, MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                string message = await response.Content.ReadAsStringAsync();
                this._displayManager.Messages.ShowMessageBox(message, LoginMessages.UnknownError, MessageBoxButton.OK, MessageBoxImage.Error);
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
        /// <summary>
        /// Changes the current language to german.
        /// </summary>
        public void ChangeLanguageToGerman()
        {
            this._languageManager.CurrentLanguage = new CultureInfo("DE");
        }
        /// <summary>
        /// Changes the current language to english.
        /// </summary>
        public void ChangeLanguageToEnglish()
        {
            this._languageManager.CurrentLanguage = new CultureInfo("EN");
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
