using System.Net.Http;
using System.Threading.Tasks;
using Xemio.SmartNotes.Shared.Entities.Users;
using Xemio.SmartNotes.Shared.Models;

namespace Xemio.SmartNotes.Client.Shared.Clients
{
    public interface IUsersClient : IClient
    {
        /// <summary>
        /// Gets the current user.
        /// </summary>
        Task<HttpResponseMessage> GetAuthorized();

        /// <summary>
        /// Creates a new <see cref="User"/>.
        /// </summary>
        /// <param name="user">The new user.</param>
        Task<HttpResponseMessage> PostUser(CreateUser user);

        /// <summary>
        /// Creates a new facebook <see cref="User"/>.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="redirectUrl">The redirect URL.</param>
        Task<HttpResponseMessage> PostFacebookUser(string code, string redirectUrl);

        /// <summary>
        /// Creates a new xemio <see cref="User"/>.
        /// </summary>
        /// <param name="emailAddress">The email address.</param>
        /// <param name="language">The language.</param>
        /// <param name="timeZone">The time zone.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        Task<HttpResponseMessage> PostXemioUser(string emailAddress, string language, string timeZone, string username, string password);

        /// <summary>
        /// Updates the <see cref="User"/>.
        /// </summary>
        /// <param name="user">The user.</param>
        Task<HttpResponseMessage> PutUser(User user);
    }
}