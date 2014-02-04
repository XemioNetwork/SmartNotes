using System.Net.Http;
using System.Threading.Tasks;

namespace Xemio.SmartNotes.Client.Shared.Clients
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