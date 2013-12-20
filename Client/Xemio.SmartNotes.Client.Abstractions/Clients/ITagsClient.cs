using System.Net.Http;
using System.Threading.Tasks;
using Xemio.SmartNotes.Models.Entities.Users;

namespace Xemio.SmartNotes.Client.Abstractions.Clients
{
    public interface ITagsClient : IClient
    {
        /// <summary>
        /// Gets the tags from the <see cref="User" />.
        /// </summary>
        /// <param name="userId">The user id.</param>
        Task<HttpResponseMessage> GetTags(int userId);
    }
}