using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xemio.SmartNotes.Abstractions.Controllers;
using Xemio.SmartNotes.Models.Entities.Users;
using Xemio.SmartNotes.Models.Models;

namespace Xemio.SmartNotes.Client.Shared.WebService
{
    /// <summary>
    /// A webservice client for the <see cref="User"/> class.
    /// </summary>
    public class UsersClient : BaseClient, IUsersController
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
        public async Task<HttpResponseMessage> GetCurrent()
        {
            this.SetAuthenticationHeader();
            this.SetLanguageHeader();

            return await this.Client.GetAsync("Users/Authorized");
        }
        /// <summary>
        /// Creates a new <see cref="User"/>.
        /// </summary>
        /// <param name="createUser">The createUser.</param>
        public async Task<HttpResponseMessage> PostUser(CreateUser createUser)
        {
            this.SetLanguageHeader();

            return await this.Client.PostAsJsonAsync("Users", createUser);
        }
        /// <summary>
        /// Updates the <see cref="User"/>.
        /// </summary>
        /// <param name="user">The user.</param>
        public async Task<HttpResponseMessage> PutUser(User user)
        {
            string requestString = await JsonConvert.SerializeObjectAsync(user);

            this.SetAuthenticationHeader(requestString);
            this.SetLanguageHeader();

            return await this.Client.PutJsonAsync("Users/Authorized", requestString);
        }
        #endregion
    }
}
