using System.Threading;
using System.Threading.Tasks;
using Raven.Client;
using Xemio.SmartNotes.Server.Abstractions.Services;
using Xemio.SmartNotes.Server.Infrastructure.Exceptions;
using Xemio.SmartNotes.Server.Infrastructure.Extensions;
using Xemio.SmartNotes.Shared.Entities.Notes;
using Xemio.SmartNotes.Shared.Entities.Users;

namespace Xemio.SmartNotes.Server.Infrastructure.Implementations.Services
{
    public class RightsService : IRightsService
    {
        #region Fields
        private readonly IDocumentSession _documentSession;
        private readonly IUserService _userService;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="RightsService" /> class.
        /// </summary>
        /// <param name="documentSession">The document session.</param>
        /// <param name="userService">The user service.</param>
        public RightsService(IDocumentSession documentSession, IUserService userService)
        {
            this._documentSession = documentSession;
            this._userService = userService;
        }
        #endregion

        #region Implementation of IRightsService
        /// <summary>
        /// Determines whether the current <see cref="User"/> has the given <paramref name="userId"/>.
        /// </summary>
        /// <param name="userId">The user id.</param>
        public bool HasCurrentUserTheUserId(int userId)
        {
            var user = this._documentSession.Load<User>(userId);
            if (user == null)
                throw new UserNotFoundException(userId);

            var currentUser = this._userService.GetCurrentUser();

            return currentUser.Id == user.Id;
        }
        /// <summary>
        /// Determines whether the current <see cref="User" /> can cacess the <see cref="Note" /> with the given id.
        /// </summary>
        /// <param name="noteId">The note id.</param>
        /// <param name="noteCanBeNull">If set to <c>true</c> the note can be null.</param>
        public bool CanCurrentUserAccessNote(int noteId, bool noteCanBeNull)
        {
            string stringId = this._documentSession.Advanced.GetStringIdFor<Note>(noteId);
            return this.CanCurrentUserAccessNote(stringId, noteCanBeNull);
        }
        /// <summary>
        /// Determines whether the current <see cref="User" /> can cacess the <see cref="Note" /> with the given id.
        /// </summary>
        /// <param name="noteId">The note id.</param>
        /// <param name="noteCanBeNull">If set to <c>true</c> the note can be null.</param>
        public bool CanCurrentUserAccessNote(string noteId, bool noteCanBeNull)
        {
            if (string.IsNullOrWhiteSpace(noteId))
                return true;

            var note = this._documentSession.Load<Note>(noteId);
            if (note == null && noteCanBeNull == false)
                throw new NoteNotFoundException(noteId);

            return this.CanCurrentUserAccess(note);
        }
        /// <summary>
        /// Determines whether the current <see cref="User" /> can access the <see cref="Folder" /> with the given id.
        /// </summary>
        /// <param name="folderId">The folder id.</param>
        /// <param name="folderCanBeNull">If set to <c>true</c> the folder can be null.</param>
        public bool CanCurrentUserAccessFolder(int folderId, bool folderCanBeNull)
        {
            string stringId = this._documentSession.Advanced.GetStringIdFor<Folder>(folderId);
            return this.CanCurrentUserAccessFolder(stringId, folderCanBeNull);
        }
        /// <summary>
        /// Determines whether the current <see cref="User" /> can access the <see cref="Folder" /> with the given id.
        /// </summary>
        /// <param name="folderId">The folder id.</param>
        /// <param name="folderCanBeNull">If set to <c>true</c> the folder can be null.</param>
        public bool CanCurrentUserAccessFolder(string folderId, bool folderCanBeNull)
        {
            if (string.IsNullOrWhiteSpace(folderId))
                return true;

            var folder = this._documentSession.Load<Folder>(folderId);
            if (folder == null && folderCanBeNull == false)
                throw new FolderNotFoundException(folderId);

            return this.CanCurrentUserAccess(folder);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Determines whether the current <see cref="User" /> can access the <typeparamref name="T" /> with the given <paramref name="id" />.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance">The instance.</param>
        public bool CanCurrentUserAccess<T>(T instance) where T : IUserSpecificEntity
        {
            User currentUser = this._documentSession.Load<User>(Thread.CurrentPrincipal.Identity.Name);

            return instance.UserId == currentUser.Id;
        }
        #endregion
    }
}
