using System.IO;
using System.Net.Http;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web.Http;

namespace Xemio.SmartNotes.Abstractions.Controllers
{
    public interface IAvatarsController : IController
    {
        /// <summary>
        /// Gets the current avatar.
        /// </summary>
        Task<HttpResponseMessage> GetAvatar();
        /// <summary>
        /// Updates the avatar.
        /// </summary>
        /// <param name="avatar">The avatar.</param>
        Task<HttpResponseMessage> PutAvatar(Stream avatar);
    }
}