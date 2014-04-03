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
        /// Updates the <see cref="User"/>.
        /// </summary>
        /// <param name="user">The user.</param>
        Task<HttpResponseMessage> PutUser(User user);
    }
}