using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using Xemio.SmartNotes.Client.Shared.Clients;
using Xemio.SmartNotes.Client.Windows.Implementations.Interaction;
using Xemio.SmartNotes.Shared.Authorization;
using Xemio.SmartNotes.Shared.Entities.Users;

namespace Xemio.SmartNotes.Client.Windows.Views.Register
{
    public class RegisterViewModel : Screen
    {
        #region Fields
        private readonly WebServiceClient _webServiceClient;
        private readonly DisplayManager _displayManager;

        private string _username;
        private string _password;
        private string _eMailAddress;
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
                    this.NotifyOfPropertyChange(() => this.CanRegister);
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
                    this.NotifyOfPropertyChange(() => this.CanRegister);
                }
            }
        }
        /// <summary>
        /// Gets or sets the E mail address.
        /// </summary>
        public string EMailAddress
        {
            get { return this._eMailAddress; }
            set
            {
                if (this._eMailAddress != value)
                {
                    this._eMailAddress = value;
                    this.NotifyOfPropertyChange(() => this.EMailAddress);
                    this.NotifyOfPropertyChange(() => this.CanRegister);
                }
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterViewModel"/> class.
        /// </summary>
        /// <param name="webServiceClient">The webservice client.</param>
        /// <param name="displayManager">The display manager.</param>
        public RegisterViewModel(WebServiceClient webServiceClient, DisplayManager displayManager)
        {
            this.DisplayName = "Xemio Notes";

            this._webServiceClient = webServiceClient;
            this._displayManager = displayManager;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Gets a value indicating whether you can execute the <see cref="Register"/> method.
        /// </summary>
        public bool CanRegister
        {
            get
            {
                return string.IsNullOrWhiteSpace(this.Username) == false &&
                       string.IsNullOrWhiteSpace(this.Password) == false &&
                       string.IsNullOrWhiteSpace(this.EMailAddress) == false;
            }
        }
        /// <summary>
        /// Registers the user.
        /// </summary>
        public async Task Register()
        {
            var user = new User
                       {
                           Username = this.Username,
                           EmailAddress = this.EMailAddress,
                           AuthorizationHash = await AuthorizationHash.CreateBaseHash(this.Username, this.Password),
                           PreferredLanguage = this._displayManager.Languages.CurrentLanguage.Name
                       };

            HttpResponseMessage response = await this._webServiceClient.Users.PostUser(user);
            if (response.StatusCode == HttpStatusCode.Created)
            {
                this.TryClose(true);
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                string message = await response.Content.ReadAsStringAsync();
                this._displayManager.Messages.ShowMessageBox(message, RegisterMessages.RegistrationFailed, MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                string message = await response.Content.ReadAsStringAsync();
                this._displayManager.Messages.ShowMessageBox(message, ClientMessages.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion
    }
}
