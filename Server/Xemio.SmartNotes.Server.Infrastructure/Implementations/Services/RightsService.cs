using System.Threading;
using System.Threading.Tasks;
using Raven.Client;
using Xemio.SmartNotes.Models.Entities.Notes;
using Xemio.SmartNotes.Models.Entities.Users;
using Xemio.SmartNotes.Server.Abstractions.Services;
using Xemio.SmartNotes.Server.Infrastructure.Exceptions;
using Xemio.SmartNotes.Server.Infrastructure.Extensions;

namespace Xemio.SmartNotes.Server.Infrastructure.Implementations.Services
{
    public class RightsService : IRightsService
    {
        #region Fields
        private readonly IAsyncDocumentSession _documentSession;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="RightsService"/> class.
        /// </summary>
        /// <param name="documentSession">The document session.</param>
        public RightsService(IAsyncDocumentSession documentSession)
        {
            this._documentSession = documentSession;
        }
        #endregion

        #region Implementation of IRightsService
        /// <summary>
        /// Determines whether the current <see cref="User"/> has the given <paramref name="userId"/>.
        /// </summary>
        /// <param name="userId">The user id.</param>
        public async Task<bool> HasCurrentUserTheUserId(int userId)
        {
            User user = await this._documentSession.LoadAsync<User>(userId);
            if (user == null)
                throw new UserNotFoundException(userId);

            User currentUser = await this._documentSession.LoadAsync<User>(Thread.CurrentPrincipal.Identity.Name);

            return currentUser == user;
        }
        /// <summary>
        /// Determines whether the current <see cref="User"/> can cacess the <see cref="Note"/> with the given id.
        /// </summary>
        /// <param name="noteId">The note id.</param>
        public async Task<bool> CanCurrentUserAccessNote(int noteId)
        {
            string stringId = this._documentSession.Advanced.GetStringIdFor<Note>(noteId);
            return await this.CanCurrentUserAccessNote(stringId);
        }
        /// <summary>
        /// Determines whether the current <see cref="User"/> can cacess the <see cref="Note"/> with the given id.
        /// </summary>
        /// <param name="noteId">The note id.</param>
        public async Task<bool> CanCurrentUserAccessNote(string noteId)
        {
            Note note = await this._documentSession.LoadAsync<Note>(noteId);
            if (note == null)
                throw new NoteNotFoundException(noteId);

            return await this.CanCurrentUserAccess(note);
        }
        /// <summary>
        /// Determines whether the current <see cref="User"/> can access the <see cref="Folder"/> with the given id.
        /// </summary>
        /// <param name="folderId">The folder id.</param>
        public async Task<bool> CanCurrentUserAccessFolder(int folderId)
        {
            string stringId = this._documentSession.Advanced.GetStringIdFor<Folder>(folderId);
            return await this.CanCurrentUserAccessFolder(stringId);
        }
        /// <summary>
        /// Determines whether the current <see cref="User"/> can access the <see cref="Folder"/> with the given id.
        /// </summary>
        /// <param name="folderId">The folder id.</param>
        public async Task<bool> CanCurrentUserAccessFolder(string folderId)
        {
            Folder folder = await this._documentSession.LoadAsync<Folder>(folderId);
            if (folder == null)
                throw new FolderNotFoundException(folderId);

            return await this.CanCurrentUserAccess(folder);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Determines whether the current <see cref="User" /> can access the <typeparamref name="T" /> with the given <paramref name="id" />.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance">The instance.</param>
        public async Task<bool> CanCurrentUserAccess<T>(T instance) where T : IUserSpecificEntity
        {
            User currentUser = await this._documentSession.LoadAsync<User>(Thread.CurrentPrincipal.Identity.Name);

            return instance.UserId == currentUser.Id;
        }
        #endregion
    }
}
