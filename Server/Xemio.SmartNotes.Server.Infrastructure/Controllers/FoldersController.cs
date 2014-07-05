using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Raven.Client;
using Xemio.SmartNotes.Server.Abstractions.Services;
using Xemio.SmartNotes.Server.Infrastructure.Deltas;
using Xemio.SmartNotes.Server.Infrastructure.Exceptions;
using Xemio.SmartNotes.Server.Infrastructure.Extensions;
using Xemio.SmartNotes.Server.Infrastructure.Filters;
using Xemio.SmartNotes.Server.Infrastructure.RavenDB.Indexes;
using Xemio.SmartNotes.Shared.Entities.Notes;
using Xemio.SmartNotes.Shared.Entities.Users;
using Xemio.SmartNotes.Shared.Extensions;

namespace Xemio.SmartNotes.Server.Infrastructure.Controllers
{
    /// <summary>
    /// Controller for the <see cref="Folder"/> class.
    /// </summary>
    [RoutePrefix("Users/Me")]
    public class FoldersController : BaseController
    {
        #region Fields
        private readonly IRightsService _rightsService;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="FoldersController"/> class.
        /// </summary>
        /// <param name="documentSession">The document session.</param>
        /// <param name="rightsService">The rights service.</param>
        /// <param name="userService">The user service.</param>
        public FoldersController(IDocumentSession documentSession, IRightsService rightsService, IUserService userService)
            : base(documentSession, userService)
        {
            this._rightsService = rightsService;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns all <see cref="Folder"/>s from the given <see cref="User"/>.
        /// </summary>
        /// <param name="parentFolderId">The parent folder id.</param>
        [Route("Folders")]
        [RequiresAuthorization]
        public HttpResponseMessage GetAllFolders(int parentFolderId = 0)
        {
            User currentUser = this.UserService.GetCurrentUser();

            string parentFolderStringId = parentFolderId > 0
                ? this.DocumentSession.Advanced.GetStringIdFor<Folder>(parentFolderId)
                : null;

            var folders = this.DocumentSession.Query<Folder, FoldersByNameAndParentFolderIdAndUserIdSortByName>()
                                              .Where(f => f.UserId == currentUser.Id && f.ParentFolderId == parentFolderStringId)
                                              .OrderBy(f => f.Name)
                                              .ToList();

            return Request.CreateResponse(HttpStatusCode.Found, folders);
        }
        /// <summary>
        /// Creates a new folder.
        /// </summary>
        /// <param name="folder">The folder.</param>
        [Route("Folders")]
        [RequiresAuthorization]
        public HttpResponseMessage PostFolder([FromBody]Folder folder)
        {
            if (folder == null)
                throw new InvalidRequestException();

            if (string.IsNullOrWhiteSpace(folder.Name))
                throw new InvalidFolderNameException();
            
            User currentUser = this.UserService.GetCurrentUser();

            folder.UserId = currentUser.Id;
            folder.CreatedDate = DateTimeOffset.Now;

            this.DocumentSession.Store(folder);

            this.Logger.DebugFormat("Created new folder '{0}' for user '{1}'.", folder.Id, currentUser.Id);

            return Request.CreateResponse(HttpStatusCode.Created, folder);
        }
        /// <summary>
        /// Updates the folder.
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <param name="folderId">The folder id.</param>
        [Route("Folders/{folderId:int}")]
        [RequiresAuthorization]
        public HttpResponseMessage PutFolder(Folder folder, int folderId)
        {
            if (folder == null)
                throw new InvalidRequestException();

            if (string.IsNullOrWhiteSpace(folder.Name))
                throw new InvalidFolderNameException();
            
            if (this._rightsService.CanCurrentUserAccessFolder(folderId, false) == false)
                throw new UnauthorizedException();

            if (this._rightsService.CanCurrentUserAccessFolder(folder.ParentFolderId, true) == false)
                throw new UnauthorizedException();

            var storedFolder = this.DocumentSession.Load<Folder>(folderId);

            storedFolder.Name = folder.Name;
            storedFolder.Tags = folder.Tags;
            storedFolder.ParentFolderId = folder.ParentFolderId;

            this.Logger.DebugFormat("Updated folder '{0}'.", storedFolder.Id);

            return Request.CreateResponse(HttpStatusCode.OK, storedFolder);
        }

        /// <summary>
        /// Patches the folder.
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <param name="folderId">The folder identifier.</param>
        [Route("Folders/{folderId:int}")]
        [RequiresAuthorization]
        public HttpResponseMessage PatchFolder(Delta<Folder> folder, int folderId)
        {
            if (folder == null)
                throw new InvalidRequestException();

            if (this._rightsService.CanCurrentUserAccessFolder(folderId, false) == false)
                throw new UnauthorizedException();

            if (folder.HasPropertyChanged(f => f.Name) &&
                string.IsNullOrWhiteSpace(folder.GetPropertyValue(f => f.Name)))
                throw new InvalidFolderNameException();

            if (folder.HasPropertyChanged(f => f.ParentFolderId) &&
                this._rightsService.CanCurrentUserAccessFolder(folder.GetPropertyValue(f => f.ParentFolderId), true) == false)
                throw new UnauthorizedException();

            if (folder.HasPropertyChanged(f => f.UserId))
                throw new InvalidRequestException();

            if (folder.HasPropertyChanged(f => f.Id))
                throw new InvalidRequestException();

            if (folder.HasPropertyChanged(f => f.CreatedDate))
                throw new InvalidRequestException();

            if (folder.HasPropertyChanged(f => f.NoteCount))
                throw new InvalidRequestException();

            var storedFolder = this.DocumentSession.Load<Folder>(folderId);
            folder.Patch(storedFolder);

            return Request.CreateResponse(HttpStatusCode.OK, storedFolder);
        }
        /// <summary>
        /// Deletes the folder.
        /// </summary>
        /// <param name="folderId">The folder id.</param>
        [Route("Folders/{folderId:int}")]
        [RequiresAuthorization]
        public HttpResponseMessage DeleteFolder(int folderId)
        {
            if (this._rightsService.CanCurrentUserAccessFolder(folderId, false) == false)
                throw new UnauthorizedException();

            var folder = this.DocumentSession
                .Load<Folder>(folderId);

            this.DocumentSession.Delete(folder);

            this.Logger.DebugFormat("Deleted folder '{0}' from user '{1}'.", folder.Id, folder.UserId);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
        #endregion
    }
}
