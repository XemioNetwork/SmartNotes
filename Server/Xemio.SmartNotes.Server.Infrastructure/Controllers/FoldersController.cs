using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Raven.Client;
using Xemio.SmartNotes.Models.Entities.Notes;
using Xemio.SmartNotes.Models.Entities.Users;
using Xemio.SmartNotes.Server.Abstractions.Services;
using Xemio.SmartNotes.Server.Infrastructure.Exceptions;
using Xemio.SmartNotes.Server.Infrastructure.Extensions;
using Xemio.SmartNotes.Server.Infrastructure.Filters;

namespace Xemio.SmartNotes.Server.Infrastructure.Controllers
{
    /// <summary>
    /// Controller for the <see cref="Folder"/> class.
    /// </summary>
    [RoutePrefix("Users/{userId:int}")]
    public class FoldersController : BaseController
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
        public FoldersController(IDocumentSession documentSession, IRightsService rightsService, IUserService userService)
            : base(documentSession)
        {
            this._rightsService = rightsService;
            this._userService = userService;
        }
        #endregion

        #region Implementation of IFoldersController
        /// <summary>
        /// Returns all <see cref="Folder"/>s from the given <see cref="User"/>.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="parentFolderId">The parent folder id.</param>
        [Route("Folders")]
        [RequiresAuthorization]
        public HttpResponseMessage GetAllFolders(int userId, string parentFolderId)
        {
            if (this._rightsService.HasCurrentUserTheUserId(userId) == false)
                throw new UnauthorizedException();

            User currentUser = this._userService.GetCurrentUser();

            var folders = this.DocumentSession.Query<Folder>().Where(f => f.UserId == currentUser.Id && f.ParentFolderId == parentFolderId).ToList();

            return Request.CreateResponse(HttpStatusCode.Found, folders);
        }
        /// <summary>
        /// Creates a new folder.
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <param name="userId">The user id.</param>
        [Route("Folders")]
        [RequiresAuthorization]
        public HttpResponseMessage PostFolder([FromBody]Folder folder, int userId)
        {
            if (folder == null)
                throw new InvalidRequestException();

            if (string.IsNullOrWhiteSpace(folder.Name))
                throw new InvalidFolderNameException();

            if (this._rightsService.HasCurrentUserTheUserId(userId) == false)
                throw new UnauthorizedException();

            User currentUser = this._userService.GetCurrentUser();

            folder.UserId = currentUser.Id;

            this.DocumentSession.Store(folder);

            this.Logger.DebugFormat("Created new folder '{0}' for user '{1}'.", folder.Id, currentUser.Id);

            return Request.CreateResponse(HttpStatusCode.Created, folder);
        }
        /// <summary>
        /// Updates the folder.
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="folderId">The folder id.</param>
        [Route("Folders/{folderId:int}")]
        [RequiresAuthorization]
        public HttpResponseMessage PutFolder([FromBody]Folder folder, int userId, int folderId)
        {
            if (folder == null)
                throw new InvalidRequestException();

            if (string.IsNullOrWhiteSpace(folder.Name))
                throw new InvalidFolderNameException();

            if (this._rightsService.HasCurrentUserTheUserId(userId) == false)
                throw new UnauthorizedException();

            if (this._rightsService.CanCurrentUserAccessFolder(folderId, false) == false)
                throw new UnauthorizedException();

            if (this._rightsService.CanCurrentUserAccessFolder(folder.ParentFolderId, true) == false)
                throw new UnauthorizedException();

            var storedFolder = this.DocumentSession.Load<Folder>(folderId);

            storedFolder.Name = folder.Name;
            storedFolder.Tags = folder.Tags;

            bool folderHasChanged = storedFolder.ParentFolderId != folder.ParentFolderId;
            if (folderHasChanged)
            {
                if (string.IsNullOrWhiteSpace(storedFolder.ParentFolderId) == false)
                {
                    var oldFolder = this.DocumentSession.Load<Folder>(storedFolder.ParentFolderId);
                    this.DocumentSession.Advanced.RemoveCascadeDelete(oldFolder, storedFolder.Id);
                }
                if (string.IsNullOrWhiteSpace(folder.ParentFolderId) == false)
                {
                    var newFolder = this.DocumentSession.Load<Folder>(folder.ParentFolderId);
                    this.DocumentSession.Advanced.AddCascadeDelete(newFolder, storedFolder.Id);
                }

                storedFolder.ParentFolderId = folder.ParentFolderId;
            }

            this.Logger.DebugFormat("Updated folder '{0}'.", storedFolder.Id);

            return Request.CreateResponse(HttpStatusCode.OK, storedFolder);
        }
        /// <summary>
        /// Deletes the folder.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="folderId">The folder id.</param>
        [Route("Folders/{folderId:int}")]
        [RequiresAuthorization]
        public HttpResponseMessage DeleteFolder(int userId, int folderId)
        {
            if (this._rightsService.HasCurrentUserTheUserId(userId) == false)
                throw new UnauthorizedException();

            if (this._rightsService.CanCurrentUserAccessFolder(folderId, false) == false)
                throw new UnauthorizedException();

            var folder = this.DocumentSession.Load<Folder>(folderId);

            this.DocumentSession.Delete(folder);

            this.Logger.DebugFormat("Deleted folder '{0}' from user '{1}'.", folder.Id, folder.UserId);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
        #endregion
    }
}
