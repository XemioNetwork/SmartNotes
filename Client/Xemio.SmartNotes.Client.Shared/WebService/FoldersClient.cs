using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
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
        /// <param name="userId">The user id.</param>
        /// <param name="parentFolderId">The parent folder id.</param>
        public Task<HttpResponseMessage> GetAllFolders(int userId, string parentFolderId)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["folder"] = parentFolderId;

            var request = this.CreateRequest(HttpMethod.Get, string.Format("Users/{0}/Folders?{1}", userId, query));
            return this.Client.SendAsync(request);
        }
        /// <summary>
        /// Creates a new <see cref="Folder"/>.
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <param name="userId">The user id.</param>
        public Task<HttpResponseMessage> PostFolder(Folder folder, int userId)
        {
            var request = this.CreateRequest(HttpMethod.Post, string.Format("Users/{0}/Folders", userId), folder);
            return this.Client.SendAsync(request);
        }
        /// <summary>
        /// Updates the <see cref="Folder"/>.
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="folderId">The folder id.</param>
        public Task<HttpResponseMessage> PutFolder(Folder folder, int userId, int folderId)
        {
            var request = this.CreateRequest(HttpMethod.Put, string.Format("Users/{0}/Folders/{1}", userId, folder), folder);
            return this.Client.SendAsync(request);
        }
        /// <summary>
        /// Deletes the <see cref="Folder"/>.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="folderId">The folder id.</param>
        public Task<HttpResponseMessage> DeleteFolder(int userId, int folderId)
        {
            var request = this.CreateRequest(HttpMethod.Delete, string.Format("Users/{0}/Folders/{1}", userId, folderId));
            return this.Client.SendAsync(request);
        }
        #endregion
    }
}
