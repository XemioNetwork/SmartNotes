using System.Net.Http;
using System.Threading.Tasks;
using Xemio.SmartNotes.Shared.Models;

namespace Xemio.SmartNotes.Client.Shared.Clients
{
    public interface IAvatarsClient : IClient
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