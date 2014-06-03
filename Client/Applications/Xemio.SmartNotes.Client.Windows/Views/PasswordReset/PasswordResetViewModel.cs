using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Windows;
using System.Windows.Forms;
using Xemio.SmartNotes.Client.Shared.Clients;
using Xemio.SmartNotes.Client.Windows.Implementations.Interaction;
using Xemio.SmartNotes.Shared.Models;

using Screen = Caliburn.Micro.Screen;

namespace Xemio.SmartNotes.Client.Windows.Views.PasswordReset
{
    public class PasswordResetViewModel : Screen
    {
        #region Fields
        private readonly WebServiceClient _webServiceClient;
        private readonly DisplayManager _windowManager;

        private string _emailAddress;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the username or email address.
        /// </summary>
        public string EmailAddress
        {
            get { return this._emailAddress; }
            set
            {
                this._emailAddress = value;

                this.NotifyOfPropertyChange(() => this.EmailAddress);
                this.NotifyOfPropertyChange(() => this.CanRequestNewPassword);
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="PasswordResetViewModel"/> class.
        /// </summary>
        /// <param name="webServiceClient">The web service client.</param>
        /// <param name="windowManager">The display manager.</param>
        public PasswordResetViewModel(WebServiceClient webServiceClient, DisplayManager windowManager)
        {
            this.DisplayName = "Xemio Notes";

            this._webServiceClient = webServiceClient;
            this._windowManager = windowManager;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Gets a value indicating whether you can request a new password.
        /// </summary>
        public bool CanRequestNewPassword
        {
            get { return string.IsNullOrWhiteSpace(this.EmailAddress) == false; }
        }
        /// <summary>
        /// Requests the new password.
        /// </summary>
        public async void RequestNewPassword()
        {
            HttpResponseMessage response = await this._webServiceClient.PasswordResets.PostPasswordReset(new CreatePasswordReset
                                                                                                             {
                                                                                                                 UsernameOrEmailAddress = this.EmailAddress
                                                                                                             });
            if (response.StatusCode == HttpStatusCode.Created)
            {
                this._windowManager.Messages.ShowMessageBox(PasswordResetMessages.FurtherInstructions, PasswordResetMessages.Successfull, MessageBoxButton.OK, MessageBoxImage.Information);
                this.TryClose(true);
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                var error = await response.Content.ReadAsAsync<HttpError>();
                this._windowManager.Messages.ShowMessageBox(error.Message, PasswordResetMessages.RequestNewPasswordFailed, MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                var error = await response.Content.ReadAsAsync<HttpError>();
                this._windowManager.Messages.ShowMessageBox(error.Message, PasswordResetMessages.UnknownError, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion
    }
}
