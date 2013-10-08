using System.Net.Http;
using System.Threading.Tasks;
using Xemio.SmartNotes.Entities.Users;
using Xemio.SmartNotes.Models.Users;

namespace Xemio.SmartNotes.Abstractions.Controllers
{
    /// <summary>
    /// Controller for the <see cref="User"/> class.
    /// </summary>
    public interface IUsersController
    {
        /// <summary>
        /// Gets the current user.
        /// </summary>
        Task<HttpResponseMessage> GetCurrent();
        /// <summary>
        /// Creates a new <see cref="User"/>.
        /// </summary>
        /// <param name="createUser">The createUser.</param>
        Task<HttpResponseMessage> PostUser(PostUser createUser);
        /// <summary>
        /// Updates the <see cref="User"/>.
        /// </summary>
        /// <param name="user">The user.</param>
        Task<HttpResponseMessage> PutUser(User user);
    }
}