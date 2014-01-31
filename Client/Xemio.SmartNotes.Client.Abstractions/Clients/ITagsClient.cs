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
        /// <param name="count">The count of tags returned.</param>
        Task<HttpResponseMessage> GetTags(int count = 20);
    }
}