using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Logging;
using Newtonsoft.Json.Linq;
using Raven.Client;
using Xemio.SmartNotes.Server.Abstractions.Authentication;
using Xemio.SmartNotes.Server.Abstractions.Social;
using Xemio.SmartNotes.Server.Infrastructure.Exceptions;
using Xemio.SmartNotes.Shared.Entities.Users;

namespace Xemio.SmartNotes.Server.Infrastructure.Implementations.Authentication
{
    public class FacebookAuthenticationProvider : IAuthenticationProvider
    {
        #region Fields
        private readonly IDocumentSession _documentSession;
        private readonly IFacebookService _facebookService;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        public ILogger Logger { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookAuthenticationProvider"/> class.
        /// </summary>
        /// <param name="documentSession">The document documentSession.</param>
        /// <param name="facebookService">The facebook service.</param>
        public FacebookAuthenticationProvider(IDocumentSession documentSession, IFacebookService facebookService)
        {
            this.Logger = NullLogger.Instance;

            this._documentSession = documentSession;
            this._facebookService = facebookService;
        }
        #endregion

        #region Implementation of IAuthenticationProvider
        /// <summary>
        /// Gets the type of the authentication.
        /// </summary>
        public AuthenticationType Type
        {
            get { return AuthenticationType.Facebook; }
        }
        /// <summary>
        /// Tries to authenticate with the specified <paramref name="data" />.
        /// </summary>
        /// <param name="data">The data.</param>
        public AuthenticationResult Authenticate(JObject data)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            var authenticationData = data.ToObject<FacebookData>();

            string accessToken = this._facebookService.ExchangeTokenForAccessToken(authenticationData.Token, authenticationData.OriginalRedirectUri);
            if (accessToken == null)
            {
                this.Logger.Debug("Could not exchange the token for an access-token while authenticating.");
                return AuthenticationResult.Failure();
            }

            FacebookUser facebookUser = this._facebookService.GetFacebookUser(accessToken);
            if (facebookUser == null)
            {
                this.Logger.Debug("Could not receive the facebook user from the specified token while authenticating.");
                return AuthenticationResult.Failure();
            }

            var authentication = this._documentSession.Query<FacebookAuthentication>()
                .Customize(f => f.WaitForNonStaleResultsAsOfLastWrite())
                .FirstOrDefault(f => f.FacebookUserId == facebookUser.Id);

            if (authentication != null)
            {
                return AuthenticationResult.Success(authentication.UserId);
            }
            else
            {
                return AuthenticationResult.Failure();
            }
        }
        /// <summary>
        /// Registers the specified <paramref name="user" /> with the specified <paramref name="data" />.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="data">The data.</param>
        public bool Register(User user, JObject data)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            if (data == null)
                throw new ArgumentNullException("data");

            var registerData = data.ToObject<FacebookData>();

            string accessToken = this._facebookService.ExchangeTokenForAccessToken(registerData.Token, registerData.OriginalRedirectUri);
            if (accessToken == null)
            {
                this.Logger.Debug("Could not exchange the token for an access-token while registering.");
                return false;
            }

            FacebookUser facebookUser = this._facebookService.GetFacebookUser(accessToken);
            if (facebookUser == null)
            {
                this.Logger.Debug("Could not receive the facebook user from the specified token while registering.");
                return false;
            }

            var authentication = new FacebookAuthentication
            {
                FacebookUserId = facebookUser.Id,
                UserId = user.Id
            };

            this._documentSession.Store(authentication);

            user.HasFacebookAuthentication = true;
            user.PreferredLanguage = facebookUser.Locale;
            user.EmailAddress = facebookUser.Email;

            return true;
        }
        /// <summary>
        /// Updates the authentication of the specified <paramref name="user" />.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="data">The data.</param>
        public void Update(User user, JObject data)
        {
            
        }
        #endregion

        #region Private
        private class FacebookData
        {
            public string Token { get; set; }

            public string OriginalRedirectUri { get; set; }
        }
        #endregion
    }
}
