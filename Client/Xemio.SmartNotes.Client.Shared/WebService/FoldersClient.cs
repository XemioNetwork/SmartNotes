using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Xemio.SmartNotes.Abstractions.Extensions;
using Xemio.SmartNotes.Client.Abstractions.Clients;
using Xemio.SmartNotes.Models.Entities.Notes;
using Xemio.SmartNotes.Models.Entities.Users;

namespace Xemio.SmartNotes.Client.Shared.WebService
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
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["folder"] = parentFolderId;

            var request = this.CreateRequest(HttpMethod.Get, string.Format("Users/Authorized/Folders?{0}", query));
            return this.Client.SendAsync(request);
        }
        /// <summary>
        /// Creates a new <see cref="Folder"/>.
        /// </summary>
        /// <param name="folder">The folder.</param>
        public Task<HttpResponseMessage> PostFolder(Folder folder)
        {
            var request = this.CreateRequest(HttpMethod.Post, "Users/Authorized/Folders", folder);
            return this.Client.SendAsync(request);
        }
        /// <summary>
        /// Updates the <see cref="Folder"/>.
        /// </summary>
        /// <param name="folder">The folder.</param>
        public Task<HttpResponseMessage> PutFolder(Folder folder)
        {
            var request = this.CreateRequest(HttpMethod.Put, string.Format("Users/Authorized/Folders/{0}", folder.Id.GetIntId()), folder);
            return this.Client.SendAsync(request);
        }
        /// <summary>
        /// Deletes the <see cref="Folder"/>.
        /// </summary>
        /// <param name="folderId">The folder id.</param>
        public Task<HttpResponseMessage> DeleteFolder(int folderId)
        {
            var request = this.CreateRequest(HttpMethod.Delete, string.Format("Users/Authorized/Folders/{0}", folderId));
            return this.Client.SendAsync(request);
        }
        #endregion
    }
}
