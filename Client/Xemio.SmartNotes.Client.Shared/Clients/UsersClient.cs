using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Xemio.SmartNotes.Shared.Entities.Users;
using Xemio.SmartNotes.Shared.Models;

namespace Xemio.SmartNotes.Client.Shared.Clients
{
    /// <summary>
    /// A webservice client for the <see cref="User"/> class.
    /// </summary>
    public class UsersClient : BaseClient, IUsersClient
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="UsersClient"/> class.
        /// </summary>
        /// <param name="baseAddress">The base address.</param>
        /// <param name="session">The application session.</param>
        public UsersClient(string baseAddress, Session session)
            : base(baseAddress, session)
        {
        }
        #endregion

        #region Implementation of IUsersController
        /// <summary>
        /// Gets the current user.
        /// </summary>
        public Task<HttpResponseMessage> GetAuthorized()
        {
            var request = this.CreateRequest(HttpMethod.Get, "Users/Me");
            return this.SendAsync(request);
        }

        /// <summary>
        /// Creates a new <see cref="User"/>.
        /// </summary>
        /// <param name="user">The new user.</param>
        public Task<HttpResponseMessage> PostUser(CreateUser user)
        {
            var request = this.CreateRequest(HttpMethod.Post, "Users", user);
            return this.SendAsync(request);
        }

        /// <summary>
        /// Creates a new facebook <see cref="User" />.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="redirectUrl">The redirect URL.</param>
        public Task<HttpResponseMessage> PostFacebookUser(string code, string redirectUrl)
        {
            return this.PostUser(new CreateUser
            {
                AuthenticationType = AuthenticationType.Facebook,
                AuthenticationData = new JObject
                {
                    {"Code", code},
                    {"RedirectUrl", redirectUrl}
                }
            });
        }

        /// <summary>
        /// Creates a new xemio <see cref="User" />.
        /// </summary>
        /// <param name="emailAddress">The email address.</param>
        /// <param name="language">The language.</param>
        /// <param name="timeZone">The time zone.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        public Task<HttpResponseMessage> PostXemioUser(string emailAddress, string language, string timeZone, string username, string password)
        {
            return this.PostUser(new CreateUser
            {
                AuthenticationType = AuthenticationType.Xemio,
                AuthenticationData = new JObject
                {
                    {"Username", username},
                    {"Password", password}
                },
                TimeZoneId = timeZone,
                EmailAddress = emailAddress,
                PreferredLanguage = language
            });
        }

        /// <summary>
        /// Updates the <see cref="User"/>.
        /// </summary>
        /// <param name="user">The user.</param>
        public Task<HttpResponseMessage> PutUser(User user)
        {
            var request = this.CreateRequest(HttpMethod.Put, "Users/Me", user);
            return this.SendAsync(request);
        }
        #endregion
    }
}
