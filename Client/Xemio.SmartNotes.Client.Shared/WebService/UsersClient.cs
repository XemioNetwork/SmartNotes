using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xemio.SmartNotes.Client.Abstractions.Clients;
using Xemio.SmartNotes.Models.Entities.Users;
using Xemio.SmartNotes.Models.Models;

namespace Xemio.SmartNotes.Client.Shared.WebService
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
            var request = this.CreateRequest(HttpMethod.Get, "Users/Authorized");
            return this.SendAsync(request);
        }
        /// <summary>
        /// Creates a new <see cref="User"/>.
        /// </summary>
        /// <param name="user">The new user.</param>
        public Task<HttpResponseMessage> PostUser(User user)
        {
            var request = this.CreateRequest(HttpMethod.Post, "Users", user);
            return this.SendAsync(request);
        }
        /// <summary>
        /// Updates the <see cref="User"/>.
        /// </summary>
        /// <param name="user">The user.</param>
        public Task<HttpResponseMessage> PutUser(User user)
        {
            var request = this.CreateRequest(HttpMethod.Put, "Users/Authorized", user);
            return this.SendAsync(request);
        }
        #endregion
    }
}
