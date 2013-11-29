using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Xemio.SmartNotes.Models.Entities.Users;
using Xemio.SmartNotes.Models.Models;

namespace Xemio.SmartNotes.Abstractions.Controllers
{
    public interface IUsersController : IController
    {
        /// <summary>
        /// Creates a new <see cref="User"/>.
        /// </summary>
        /// <param name="createUser">The createUser.</param>
        Task<HttpResponseMessage> PostUser(CreateUser createUser);
        /// <summary>
        /// Gets the current user.
        /// </summary>
        Task<HttpResponseMessage> GetAuthorized();
        /// <summary>
        /// Updates the <see cref="User"/>.
        /// </summary>
        /// <param name="user">The user.</param>
        Task<HttpResponseMessage> PutUser(User user);
    }
}