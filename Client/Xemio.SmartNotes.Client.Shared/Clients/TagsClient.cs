using System.Net.Http;
using System.Threading.Tasks;
using Xemio.SmartNotes.Shared.Entities.Users;

namespace Xemio.SmartNotes.Client.Shared.Clients
{
    public class TagsClient : BaseClient, ITagsClient
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="TagsClient"/> class.
        /// </summary>
        /// <param name="baseAddress">The base address.</param>
        /// <param name="session">The application session.</param>
        public TagsClient(string baseAddress, Session session)
            : base(baseAddress, session)
        {
        }
        #endregion

        #region Implementation of ITagsController
        /// <summary>
        /// Gets the tags from the <see cref="User" />.
        /// </summary>
        /// <param name="count">The count of tags returned.</param>
        public async Task<HttpResponseMessage> GetTags(int count = 20)
        {
            var query = new HttpQueryBuilder();
            query.AddParameter("count", count);

            var request = await this.CreateRequest(HttpMethod.Get, string.Format("Users/Authorized/Tags{0}", query));
            return await this.SendAsync(request);
        }
        #endregion
    }
}
