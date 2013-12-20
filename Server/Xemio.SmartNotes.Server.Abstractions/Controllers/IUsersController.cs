using System.Net.Http;
using Xemio.SmartNotes.Models.Entities.Users;

namespace Xemio.SmartNotes.Server.Abstractions.Controllers
{
    public interface IUsersController : IController
    {
        /// <summary>
        /// Creates a new <see cref="User"/>.
        /// </summary>
        /// <param name="user">The new user.</param>
        HttpResponseMessage PostUser(User user);
        /// <summary>
        /// Gets the current user.
        /// </summary>
        HttpResponseMessage GetAuthorized();
        /// <summary>
        /// Updates the <see cref="User"/>.
        /// </summary>
        /// <param name="user">The user.</param>
        HttpResponseMessage PutUser(User user);
    }
}