using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using Castle.Core.Logging;
using CuttingEdge.Conditions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog.Conditions;
using NodaTime;
using Raven.Client;
using Raven.Json.Linq;
using Xemio.SmartNotes.Server.Abstractions.Social;
using Xemio.SmartNotes.Server.Infrastructure.RavenDB.Indexes;
using Xemio.SmartNotes.Shared.Entities.Users;

namespace Xemio.SmartNotes.Server.Infrastructure.Implementations.Social
{
    public class FacebookService : IFacebookService
    {
        #region Fields
        private readonly IDocumentStore _documentStore;
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
        /// <param name="documentStore">The document store.</param>
        public FacebookService(string appId, string appSecret, IDocumentStore documentStore)
        {
            Condition.Requires(appId, "appId")
                .IsNotNull();
            Condition.Requires(appSecret, "appSecret")
                .IsNotNull();

            this.Logger = NullLogger.Instance;

            this._documentStore = documentStore;
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
            Condition.Requires(token, "token")
                .IsNotNullOrWhiteSpace();
            Condition.Requires(redirectUri, "redirectUri")
                .IsNotNullOrWhiteSpace();

            try
            {
                using (var documentSession = this._documentStore.OpenSession())
                {
                    var cached = documentSession.Query<CachedFacebookTokenExchange, CachedFacebookTokenExchangesByToken>()
                                                .FirstOrDefault(f => f.Token == token);
                    if (cached != null)
                    {
                        return cached.AccessToken;
                    }
                    
                    string exchangeResult = new HttpClient().GetStringAsync(this.GetTokenExchangeUrl(token, redirectUri)).Result;
                    string accessToken = HttpUtility.ParseQueryString(exchangeResult)["access_token"];

                    this.CacheToken(documentSession, token, accessToken);

                    documentSession.SaveChanges();

                    return accessToken;
                }
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
            Condition.Requires(accessToken, "accessToken")
                .IsNotNullOrWhiteSpace();

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
            Condition.Requires(token, "token")
                .IsNotNullOrWhiteSpace();
            Condition.Requires(redirectUri, "redirectUri")
                .IsNotNullOrWhiteSpace();

            return string.Format("https://graph.facebook.com/oauth/access_token?client_id={0}&redirect_uri={1}&client_secret={2}&code={3}",
                                  this.AppId, redirectUri, this.AppSecret, token);
        }
        /// <summary>
        /// Returns the user URL.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        private string GetUserUrl(string accessToken)
        {
            Condition.Requires(accessToken, "accessToken")
                .IsNotNullOrWhiteSpace();

            return string.Format("https://graph.facebook.com/me?access_token={0}", accessToken);
        }
        /// <summary>
        /// Caches the token.
        /// </summary>
        /// <param name="documentSession">The document session.</param>
        /// <param name="token">The token.</param>
        /// <param name="accessToken">The access token.</param>
        private void CacheToken(IDocumentSession documentSession, string token, string accessToken)
        {
            var cached = new CachedFacebookTokenExchange
            {
                Token = token,
                AccessToken = accessToken
            };
            documentSession.Store(cached);

            RavenJObject metadata = documentSession.Advanced.GetMetadataFor(cached);
            metadata["Raven-Expiration-Date"] = new RavenJValue(DateTimeOffset.UtcNow.AddMinutes(3));
        }
        #endregion
    }
}
