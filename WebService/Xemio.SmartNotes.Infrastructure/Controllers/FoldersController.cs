using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using Raven.Client;
using Xemio.SmartNotes.Abstractions.Controllers;
using Xemio.SmartNotes.Core.Exceptions;
using Xemio.SmartNotes.Core.Services;
using Xemio.SmartNotes.Entities.Notes;
using Xemio.SmartNotes.Entities.Users;
using Xemio.SmartNotes.Infrastructure.Extensions;
using Xemio.SmartNotes.Infrastructure.Authentication;
using Xemio.SmartNotes.Infrastructure.Raven.Indexes;

namespace Xemio.SmartNotes.Infrastructure.Controllers
{
    /// <summary>
    /// Controller for the <see cref="Folder"/> class.
    /// </summary>
    [RoutePrefix("Users/{userId:int}")]
    public class FoldersController : BaseController, IFoldersController
    {
        #region Fields
        private readonly IRightsService _rightsService;
        private readonly IUserService _userService;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="FoldersController"/> class.
        /// </summary>
        /// <param name="documentSession">The document session.</param>
        /// <param name="rightsService">The rights service.</param>
        /// <param name="userService">The user service.</param>
        public FoldersController(IAsyncDocumentSession documentSession, IRightsService rightsService, IUserService userService)
            : base(documentSession)
        {
            this._rightsService = rightsService;
            this._userService = userService;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns all <see cref="Folder"/>s from the given <see cref="User"/>.
        /// </summary>
        /// <param name="userId">The user id.</param>
        [GET("Folders")]
        [RequiresAuthentication]
        public async Task<HttpResponseMessage> GetAllFolders(int userId)
        {
            if (await this._rightsService.HasCurrentUserTheUserId(userId) == false)
                throw new UnauthorizedException();

            User currentUser = await this._userService.GetCurrentUser();

            var query = this.DocumentSession.Query<Folder, FoldersByUserId>().Where(f => f.UserId == currentUser.Id);

            List<Folder> result = new List<Folder>();
            using (var enumerator = await this.DocumentSession.Advanced.StreamAsync(query))
            {
                while (await enumerator.MoveNextAsync())
                {
                    result.Add(enumerator.Current.Document);
                }
            }

            return Request.CreateResponse(HttpStatusCode.Found, result);
        }
        /// <summary>
        /// Creates a new folder.
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <param name="userId">The user id.</param>
        [POST("Folders")]
        [RequiresAuthentication]
        public async Task<HttpResponseMessage> PostFolder([FromBody]Folder folder, int userId)
        {
            if (folder == null)
                throw new InvalidRequestException();

            if (string.IsNullOrWhiteSpace(folder.Name))
                throw new InvalidFolderNameException();

            if (await this._rightsService.HasCurrentUserTheUserId(userId) == false)
                throw new UnauthorizedException();

            User currentUser = await this._userService.GetCurrentUser();

            folder.UserId = currentUser.Id;

            await this.DocumentSession.StoreAsync(folder);

            return Request.CreateResponse(HttpStatusCode.Created, folder);
        }
        /// <summary>
        /// Updates the folder.
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="folderId">The folder id.</param>
        [PUT("Folders/{folderId:int}")]
        [RequiresAuthentication]
        public async Task<HttpResponseMessage> PutFolder([FromBody]Folder folder, int userId, int folderId)
        {
            if (folder == null)
                throw new InvalidRequestException();

            if (string.IsNullOrWhiteSpace(folder.Name))
                throw new InvalidFolderNameException();

            if (await this._rightsService.HasCurrentUserTheUserId(userId) == false)
                throw new UnauthorizedException();

            if (await this._rightsService.CanCurrentUserAccessFolder(folderId) == false)
                throw new UnauthorizedException();
            
            Folder storedFolder = await this.DocumentSession.LoadAsync<Folder>(folderId);

            storedFolder.Name = folder.Name;
            storedFolder.Tags = folder.Tags;

            return Request.CreateResponse(HttpStatusCode.OK, storedFolder);
        }
        /// <summary>
        /// Deletes the folder.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="folderId">The folder id.</param>
        [DELETE("Folders/{folderId:int}")]
        [RequiresAuthentication]
        public async Task<HttpResponseMessage> DeleteFolder(int userId, int folderId)
        {
            if (await this._rightsService.HasCurrentUserTheUserId(userId) == false)
                throw new UnauthorizedException();

            if (await this._rightsService.CanCurrentUserAccessFolder(folderId) == false)
                throw new UnauthorizedException();

            Folder folder = await this.DocumentSession.LoadAsync<Folder>(folderId);

            this.DocumentSession.Delete(folder);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
        #endregion
    }
}
