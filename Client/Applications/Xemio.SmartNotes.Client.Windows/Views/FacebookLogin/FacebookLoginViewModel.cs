using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using Newtonsoft.Json.Linq;
using Xemio.SmartNotes.Client.Shared.Clients;
using Xemio.SmartNotes.Client.Shared.Extensions;
using Xemio.SmartNotes.Client.Windows.Implementations.Interaction;
using Xemio.SmartNotes.Shared.Entities.Users;
using Xemio.SmartNotes.Shared.Models;

namespace Xemio.SmartNotes.Client.Windows.Views.FacebookLogin
{
    public class FacebookLoginViewModel : Screen
    {
        #region Fields
        private readonly WebServiceClient _webServiceClient;
        private readonly DisplayManager _displayManager;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the application identifier.
        /// </summary>
        public string AppId { get; private set; }
        /// <summary>
        /// Gets the code.
        /// </summary>
        public AuthenticationToken Token { get; private set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookLoginViewModel"/> class.
        /// </summary>
        /// <param name="appId">The application identifier.</param>
        /// <param name="webServiceClient">The web service client.</param>
        /// <param name="displayManager">The display manager.</param>
        public FacebookLoginViewModel(string appId, WebServiceClient webServiceClient, DisplayManager displayManager)
        {
            this.DisplayName = "Xemio Notes";

            if (string.IsNullOrWhiteSpace(appId))
                throw new ArgumentNullException("appId");
            if (webServiceClient == null)
                throw new ArgumentNullException("webServiceClient");

            this.AppId = appId;
            this._webServiceClient = webServiceClient;
            this._displayManager = displayManager;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Called when the user has logged in.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="redirectUrl">The redirect url used to receive the code.</param>
        /// <param name="isRetry">Indicating whether we're retrying to authenticate.</param>
        /// <param name="registerMessage">The message we got from the register call.</param>
        public async Task UserLoggedIn(string code, string redirectUrl, bool isRetry = false, string registerMessage = null)
        {
            HttpResponseMessage tokenResponse = await this._webServiceClient.Tokens.PostFacebook(code, redirectUrl);
            if (tokenResponse.StatusCode == HttpStatusCode.OK)
            {
                this.Token = await tokenResponse.Content.ReadAsAsync<AuthenticationToken>();
                this.TryClose(true);
            }
            else if (tokenResponse.StatusCode == HttpStatusCode.Unauthorized && isRetry == false)
            {
                HttpResponseMessage registerResponse = await this._webServiceClient.Users.PostFacebookUser(code, redirectUrl);
                string message = await registerResponse.Content.ReadAsStringAsync();

                await this.UserLoggedIn(code, redirectUrl, true, message);
            }
            else
            {
                this._displayManager.Messages.ShowMessageBox(registerMessage, FacebookLoginMessages.UnknownError, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// Called whent he user declined access to his account.
        /// </summary>
        public void UserCanceled()
        {
            this.TryClose(false);
        }
        #endregion
    }
}
