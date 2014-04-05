﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using Castle.Core.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NodaTime;
using Xemio.SmartNotes.Server.Abstractions.Social;

namespace Xemio.SmartNotes.Server.Infrastructure.Implementations.Social
{
    public class FacebookService : IFacebookService
    {
        #region Fields
        private readonly MemoryCache _tokenCache;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        public ILogger Logger { get; set; }
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
            this.Logger = NullLogger.Instance;

            if (appId == null)
                throw new ArgumentNullException("appId");
            if (appSecret == null)
                throw new ArgumentNullException("appSecret");

            this._tokenCache = new MemoryCache("FacebookAccessTokenCache");
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
            try
            {
                //We need this cache here because in the login process the user might yet not exist
                //So another request comes in and registers it
                //But now the token is already used and we can't get a new one from facebook
                //So we just cache it for a short amount of time
                if (this._tokenCache.Contains(token))
                    return (string)this._tokenCache.Get(token);
                    
                string exchangeResult = new HttpClient().GetStringAsync(this.GetTokenExchangeUrl(token, redirectUri)).Result;
                string accessToken = HttpUtility.ParseQueryString(exchangeResult)["access_token"];

                this._tokenCache.Add(token, accessToken, DateTimeOffset.UtcNow.AddMinutes(2));

                return accessToken;
            }
            catch (Exception exception)
            {
                this.Logger.Error("Error when exchanging the facebook code for an access token.", exception);
                return null;
            }
        }
        /// <summary>
        /// Gets the facebook user.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        public FacebookUser GetFacebookUser(string accessToken)
        {
            string userResult = new HttpClient().GetStringAsync(this.GetUserUrl(accessToken)).Result;
            JObject user = JObject.Parse(userResult);

            return new FacebookUser
            {
                Email = user.Value<string>("email"),
                Id = user.Value<string>("id"),
                Locale = user.Value<string>("locale").Replace("_", "-")
            };
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
        /// <summary>
        /// Returns the user URL.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        private string GetUserUrl(string accessToken)
        {
            return string.Format("https://graph.facebook.com/me?access_token={0}", accessToken);
        }
        #endregion
    }
}
