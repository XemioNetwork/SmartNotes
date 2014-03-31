using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Xemio.SmartNotes.Server.Abstractions.Social;

namespace Xemio.SmartNotes.Server.Infrastructure.Implementations.Social
{
    public class FacebookService : IFacebookService
    {
        #region Properties
        /// <summary>
        /// Gets or sets the application identifier.
        /// </summary>
        public string AppId { get; set; }
        /// <summary>
        /// Gets or sets the application secret.
        /// </summary>
        public string AppSecret { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookService"/> class.
        /// </summary>
        /// <param name="appId">The application identifier.</param>
        /// <param name="appSecret">The application secret.</param>
        public FacebookService(string appId, string appSecret)
        {
            if (appId == null)
                throw new ArgumentNullException("appId");
            if (appSecret == null)
                throw new ArgumentNullException("appSecret");

            this.AppId = appId;
            this.AppSecret = appSecret;
        }
        #endregion

        #region Implementation of IFacebookService
        /// <summary>
        /// Exchanges the specified <paramref name="token" /> for an access token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="redirectUri">The redirect URI.</param>
        public string ExchangeTokenForAccessToken(string token, string redirectUri)
        {
            string exchangeResult = new HttpClient().GetStringAsync(this.GetTokenExchangeUrl(token, redirectUri)).Result;
            return HttpUtility.ParseQueryString(exchangeResult)["access_token"];
        }
        /// <summary>
        /// Gets the facebook user.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        public FacebookUser GetFacebookUser(string accessToken)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Returns the token exchange URL.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="redirectUri">The redirect URI.</param>
        private string GetTokenExchangeUrl(string token, string redirectUri)
        {
            return string.Format("https://graph.facebook.com/oauth/access_token?client_id={0}&redirect_uri={1}&client_secret={2}&code={3}",
                                  this.AppId, redirectUri, this.AppSecret, token);
        }
        #endregion
    }
}
