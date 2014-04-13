using System.Net.Http;
using System.Threading.Tasks;
using Xemio.SmartNotes.Shared.Models;

namespace Xemio.SmartNotes.Client.Shared.Clients
{
    public class AvatarsClient : BaseClient, IAvatarsClient
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="AvatarsClient"/> class.
        /// </summary>
        /// <param name="baseAddress">The base address.</param>
        /// <param name="session">The application session.</param>
        public AvatarsClient(string baseAddress, Session session)
            : base(baseAddress, session)
        {
        }
        #endregion

        #region Implementation of IAvatarsController
        /// <summary>
        /// Gets the current avatar.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public Task<HttpResponseMessage> GetAvatar(int width = 0, int height = 0)
        {
            var query = new HttpQueryBuilder();
            query.AddParameter("width", width);
            query.AddParameter("height", height);

            var request = this.CreateRequest(HttpMethod.Get, string.Format("Users/Me/Avatar{0}", query));
            return this.SendAsync(request);
        }
        /// <summary>
        /// Updates the avatar.
        /// </summary>
        /// <param name="avatar">The avatar.</param>
        public Task<HttpResponseMessage> PutAvatar(CreateAvatar avatar)
        {
            var request = this.CreateRequest(HttpMethod.Get, "Users/Me/Avatar", avatar);
            return this.SendAsync(request);
        }
        #endregion
    }
}
