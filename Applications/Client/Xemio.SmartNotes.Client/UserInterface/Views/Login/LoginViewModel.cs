using System;
using System.Net;
using System.Net.Http;
using System.Windows;
using Caliburn.Micro;
using Xemio.SmartNotes.Abstractions.Controllers;
using Xemio.SmartNotes.Client.Data;
using Xemio.SmartNotes.Entities.Users;

namespace Xemio.SmartNotes.Client.UserInterface.Views.Login
{
    public class LoginViewModel : Screen
    {
        #region Fields
        private readonly IUsersController _usersController;
        private readonly Session _session;

        private string _username;
        private string _password;
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
                }
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="LoginViewModel" /> class.
        /// </summary>
        /// <param name="usersController">The users controller.</param>
        /// <param name="session">The application session.</param>
        public LoginViewModel(IUsersController usersController, Session session)
        {
            this.DisplayName = "Xemio - Smart Notes - Anmeldung";

            this._usersController = usersController;
            this._session = session;
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
                this._session.User = await response.Content.ReadAsAsync<User>();
                this.TryClose(true);
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                MessageBox.Show(string.Format("Die Anmeldung ist fehlgeschlagen.{0}Überprüfen Sie Ihren Benutzernamen und Passwort.", Environment.NewLine), "Anmeldung fehlgeschlagen", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        #endregion
    }
}
