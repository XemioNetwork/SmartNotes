﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using Xemio.SmartNotes.Client.Shared.Clients;
using Xemio.SmartNotes.Client.Shared.Extensions;
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

        private string _usernameOrEmailAddress;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the username or email address.
        /// </summary>
        public string UsernameOrEmailAddress
        {
            get { return this._usernameOrEmailAddress; }
            set
            {
                this._usernameOrEmailAddress = value;

                this.NotifyOfPropertyChange(() => this.UsernameOrEmailAddress);
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
            get { return string.IsNullOrWhiteSpace(this.UsernameOrEmailAddress) == false; }
        }
        /// <summary>
        /// Requests the new password.
        /// </summary>
        public async void RequestNewPassword()
        {
            HttpResponseMessage response = await this._webServiceClient.PasswordResets.PostPasswordReset(new CreatePasswordReset
                                                                                                             {
                                                                                                                 UsernameOrEmailAddress = this.UsernameOrEmailAddress
                                                                                                             });
            if (response.StatusCode == HttpStatusCode.Created)
            {
                this._windowManager.Messages.ShowMessageBox(PasswordResetMessages.FurtherInstructions, PasswordResetMessages.Successfull, MessageBoxButton.OK, MessageBoxImage.Information);
                this.TryClose(true);
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                var error = await response.Content.ReadAsAsync<Error>();
                this._windowManager.Messages.ShowMessageBox(error.Message, PasswordResetMessages.RequestNewPasswordFailed, MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                var error = await response.Content.ReadAsAsync<Error>();
                this._windowManager.Messages.ShowMessageBox(error.Message, PasswordResetMessages.UnknownError, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion
    }
}
