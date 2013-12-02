using System.IO;
using System.Net.Http;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web.Http;
using Xemio.SmartNotes.Models.Models;

namespace Xemio.SmartNotes.Abstractions.Controllers
{
    public interface IAvatarsController : IController
    {
        /// <summary>
        /// Gets the current avatar.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        Task<HttpResponseMessage> GetAvatar(int width = 0, int height = 0);
        /// <summary>
        /// Updates the avatar.
        /// </summary>
        /// <param name="avatar">The avatar.</param>
        Task<HttpResponseMessage> PutAvatar(CreateAvatar avatar);
    }
}