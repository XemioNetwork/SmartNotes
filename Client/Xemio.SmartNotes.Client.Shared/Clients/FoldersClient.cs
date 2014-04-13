using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Xemio.SmartNotes.Shared.Entities.Notes;
using Xemio.SmartNotes.Shared.Entities.Users;
using Xemio.SmartNotes.Shared.Extensions;

namespace Xemio.SmartNotes.Client.Shared.Clients
{
    public class FoldersClient : BaseClient, IFoldersClient
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="FoldersClient"/> class.
        /// </summary>
        /// <param name="baseAddress">The base address.</param>
        /// <param name="session">The application session.</param>
        public FoldersClient(string baseAddress, Session session)
            : base(baseAddress, session)
        {
        }
        #endregion

        #region Implementation of IFoldersController
        /// <summary>
        /// Returns all <see cref="Folder"/>s from the given <see cref="User"/>.
        /// </summary>
        /// <param name="parentFolderId">The parent folder id.</param>
        public Task<HttpResponseMessage> GetAllFolders(string parentFolderId)
        {
            var query = new HttpQueryBuilder();

            if (parentFolderId != null)
                query.AddParameter("parentFolderId", parentFolderId.GetIntId());

            var request = this.CreateRequest(HttpMethod.Get, string.Format("Users/Me/Folders{0}", query));
            return this.SendAsync(request);
        }
        /// <summary>
        /// Creates a new <see cref="Folder"/>.
        /// </summary>
        /// <param name="folder">The folder.</param>
        public Task<HttpResponseMessage> PostFolder(Folder folder)
        {
            folder.UserId = this.Session.User.Id;

            var request = this.CreateRequest(HttpMethod.Post, "Users/Me/Folders", folder);
            return this.SendAsync(request);
        }

        /// <summary>
        /// Updates the <see cref="Folder"/>.
        /// </summary>
        /// <param name="folder">The folder.</param>
        public Task<HttpResponseMessage> PutFolder(Folder folder)
        {
            var request = this.CreateRequest(HttpMethod.Put, string.Format("Users/Me/Folders/{0}", folder.Id.GetIntId()), folder);
            return this.SendAsync(request);
        }
        /// <summary>
        /// Deletes the <see cref="Folder"/>.
        /// </summary>
        /// <param name="folderId">The folder id.</param>
        public Task<HttpResponseMessage> DeleteFolder(string folderId)
        {
            var request = this.CreateRequest(HttpMethod.Delete, string.Format("Users/Me/Folders/{0}", folderId.GetIntId()));
            return this.SendAsync(request);
        }
        /// <summary>
        /// Patches the <see cref="Folder" />.
        /// </summary>
        /// <param name="folderId">The folder identifier.</param>
        /// <param name="data">The data.</param>
        public Task<HttpResponseMessage> PatchFolder(string folderId, JObject data)
        {
            var request = this.CreateRequest(new HttpMethod("PATCH"), string.Format("Users/Me/Folders/{0}", folderId.GetIntId()), data);
            return this.SendAsync(request);
        }
        #endregion
    }
}
