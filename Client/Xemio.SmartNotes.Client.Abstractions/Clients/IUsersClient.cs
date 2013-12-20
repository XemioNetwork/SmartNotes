using System.Net.Http;
using System.Threading.Tasks;
using Xemio.SmartNotes.Models.Entities.Users;

namespace Xemio.SmartNotes.Client.Abstractions.Clients
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
        Task<HttpResponseMessage> PostUser(User user);

        /// <summary>
        /// Updates the <see cref="User"/>.
        /// </summary>
        /// <param name="user">The user.</param>
        Task<HttpResponseMessage> PutUser(User user);
    }
}