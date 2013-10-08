using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xemio.SmartNotes.Abstractions.Controllers;
using Xemio.SmartNotes.Client.Data;
using Xemio.SmartNotes.Client.Extensions;
using Xemio.SmartNotes.Entities.Notes;
using Xemio.SmartNotes.Entities.Users;

namespace Xemio.SmartNotes.Client.WebServices
{
    public class FoldersClient : BaseClient, IFoldersController
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="FoldersClient"/> class.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <param name="baseAddress">The base address.</param>
        public FoldersClient(Session session, string baseAddress)
            : base(session, baseAddress)
        {
        }
        #endregion

        #region Implementation of IFoldersController
        /// <summary>
        /// Returns all <see cref="Folder"/>s from the given <see cref="User"/>.
        /// </summary>
        /// <param name="userId">The user id.</param>
        public async Task<HttpResponseMessage> GetAllFolders(int userId)
        {
            this.SetAuthenticationHeader(string.Empty);
            return await this.Client.GetAsync(string.Format("Users/{0}/Folders", userId));
        }
        /// <summary>
        /// Creates a new <see cref="Folder"/>.
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <param name="userId">The user id.</param>
        public async Task<HttpResponseMessage> PostFolder(Folder folder, int userId)
        {
            string requestString = await JsonConvert.SerializeObjectAsync(folder);

            this.SetAuthenticationHeader(requestString);
            return await this.Client.PostJsonAsync(string.Format("Users/{0}/Folders", userId), requestString);
        }
        /// <summary>
        /// Updates the <see cref="Folder"/>.
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="folderId">The folder id.</param>
        public async Task<HttpResponseMessage> PutFolder(Folder folder, int userId, int folderId)
        {
            string requestString = await JsonConvert.SerializeObjectAsync(folder);

            this.SetAuthenticationHeader(requestString);
            return await this.Client.PutJsonAsync(string.Format("Users/{0}/Folders/{1}", userId, folder), requestString);
        }
        /// <summary>
        /// Deletes the <see cref="Folder"/>.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="folderId">The folder id.</param>
        public async Task<HttpResponseMessage> DeleteFolder(int userId, int folderId)
        {
            this.SetAuthenticationHeader();
            return await this.Client.DeleteAsync(string.Format("Users/{0}/Folders/{1}", userId, folderId));
        }
        #endregion
    }
}
