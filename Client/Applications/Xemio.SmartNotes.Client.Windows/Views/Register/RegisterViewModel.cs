using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Xemio.SmartNotes.Abstractions.Authorization;
using Xemio.SmartNotes.Abstractions.Controllers;
using Xemio.SmartNotes.Models.Models;

namespace Xemio.SmartNotes.Client.Windows.Views.Register
{
    public class RegisterViewModel : Screen
    {

        #region Fields
        private readonly IUsersController _usersController;

        private string _username;
        private string _password;
        private string _eMailAddress;
        private bool _canRegister;

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
        /// <param name="usersController">The users controller.</param>
        public RegisterViewModel(IUsersController usersController)
        {
            this._usersController = usersController;
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
        public void Register()
        {
            var user = new CreateUser
                       {
                           Username = this.Username,
                           EMailAddress = this.EMailAddress,
                           AuthorizationHash = AuthorizationHash.CreateBaseHash(this.Username, this.Password)
                       };
        }
        #endregion
    }
}
