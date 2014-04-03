using System;
using System.Dynamic;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using Caliburn.Micro;
using Xemio.CommonLibrary.Storage;
using Xemio.SmartNotes.Client.Shared.Clients;
using Xemio.SmartNotes.Client.Shared.Extensions;
using Xemio.SmartNotes.Client.Shared.Settings;
using Xemio.SmartNotes.Client.Windows.Data;
using Xemio.SmartNotes.Client.Windows.Implementations.Interaction;
using Xemio.SmartNotes.Client.Windows.Views.FacebookLogin;
using Xemio.SmartNotes.Client.Windows.Views.PasswordReset;
using Xemio.SmartNotes.Client.Windows.Views.Register;
using Xemio.SmartNotes.Client.Windows.Views.XemioLogin;
using Xemio.SmartNotes.Shared.Entities.Users;

namespace Xemio.SmartNotes.Client.Windows.Views.Login
{
    public class LoginViewModel : Screen
    {
        #region Fields
        private readonly WebServiceClient _webServiceClient;
        private readonly ILanguageManager _languageManager;
        private readonly DisplayManager _displayManager;
        #endregion
        
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="LoginViewModel" /> class.
        /// </summary>
        /// <param name="webServiceClient">The webservice client.</param>
        /// <param name="languageManager">The language manager.</param>
        /// <param name="displayManager">The display manager.</param>
        public LoginViewModel(WebServiceClient webServiceClient, ILanguageManager languageManager, DisplayManager displayManager)
        {
            this.DisplayName = "Xemio Notes";

            this._webServiceClient = webServiceClient;
            this._languageManager = languageManager;
            this._displayManager = displayManager;
        }
        #endregion
        
        #region Methods
        /// <summary>
        /// Shows the facebook login window.
        /// </summary>
        public async Task FacebookLogin()
        {
            var viewModel = IoC.Get<FacebookLoginViewModel>();
            
            dynamic settings = new ExpandoObject();
            settings.ResizeMode = ResizeMode.CanMinimize;

            bool? result = this._displayManager.Windows.ShowDialog(viewModel, null, settings);

            if (result.GetValueOrDefault())
            {
                await this.FinishLogin(viewModel.Token);
            }
        }
        /// <summary>
        /// Shows the xemio notes login window.
        /// </summary>
        public async Task XemioNotesLogin()
        {
            var viewModel = IoC.Get<XemioLoginViewModel>();

            dynamic settings = new ExpandoObject();
            settings.ResizeMode = ResizeMode.CanMinimize;

            bool? result = this._displayManager.Windows.ShowDialog(viewModel, null, settings);

            if (result.GetValueOrDefault())
            {
                await this.FinishLogin(viewModel.Token);
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Finishes the login process.
        /// </summary>
        /// <param name="token">The token.</param>
        private async Task FinishLogin(AuthenticationToken token)
        {
            this._webServiceClient.Session.Token = token;

            HttpResponseMessage userResponse = await this._webServiceClient.Users.GetAuthorized();
            if (userResponse.StatusCode == HttpStatusCode.Found)
            {
                var user = await userResponse.Content.ReadAsAsync<User>();
                this._webServiceClient.Session.User = user;

                this._languageManager.SetLanguageFromUser(user);

                this.TryClose(true);
            }
            else
            {
                string message = await userResponse.Content.ReadAsStringAsync();
                this._displayManager.Messages.ShowMessageBox(message, LoginMessages.UnknownError, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion
    }
}
