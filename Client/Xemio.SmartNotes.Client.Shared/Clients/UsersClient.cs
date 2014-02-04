﻿using System.Net.Http;
using System.Threading.Tasks;
using Xemio.SmartNotes.Shared.Entities.Users;

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
        public async Task<HttpResponseMessage> GetAuthorized()
        {
            var request = await this.CreateRequest(HttpMethod.Get, "Users/Authorized");
            return await this.SendAsync(request);
        }
        /// <summary>
        /// Creates a new <see cref="User"/>.
        /// </summary>
        /// <param name="user">The new user.</param>
        public async Task<HttpResponseMessage> PostUser(User user)
        {
            var request = await this.CreateRequest(HttpMethod.Post, "Users", user);
            return await this.SendAsync(request);
        }
        /// <summary>
        /// Updates the <see cref="User"/>.
        /// </summary>
        /// <param name="user">The user.</param>
        public async Task<HttpResponseMessage> PutUser(User user)
        {
            var request = await this.CreateRequest(HttpMethod.Put, "Users/Authorized", user);
            return await this.SendAsync(request);
        }
        #endregion
    }
}