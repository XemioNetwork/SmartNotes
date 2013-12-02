using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Raven.Client;
using Xemio.SmartNotes.Abstractions.Controllers;
using Xemio.SmartNotes.Models.Entities.Notes;
using Xemio.SmartNotes.Models.Entities.Users;
using Xemio.SmartNotes.Server.Abstractions.Services;
using Xemio.SmartNotes.Server.Infrastructure.Exceptions;
using Xemio.SmartNotes.Server.Infrastructure.Extensions;
using Xemio.SmartNotes.Server.Infrastructure.Filters;
using Xemio.SmartNotes.Server.Infrastructure.Raven.Indexes;

namespace Xemio.SmartNotes.Server.Infrastructure.Controllers
{
    /// <summary>
    /// Controller for the <see cref="Note"/> class.
    /// </summary>
    [RoutePrefix("Users/{userId:int}")]
    public class NotesController : BaseController, INotesController
    {
        #region Fields
        private readonly IRightsService _rightsService;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="NotesController"/> class.
        /// </summary>
        /// <param name="documentSession">The document session.</param>
        /// <param name="rightsService">The right service.</param>
        public NotesController(IAsyncDocumentSession documentSession, IRightsService rightsService)
            : base(documentSession)
        {
            this._rightsService = rightsService;
        }
        #endregion

        #region Implementation of INotesController
        /// <summary>
        /// Returns all <see cref="Note" />s from the given <see cref="Folder" />.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="folderId">The note id.</param>
        [Route("Notes")]
        [RequiresAuthorization]
        public async Task<HttpResponseMessage> GetAllNotes(int userId, int folderId)
        {
            if (await this.DocumentSession.LoadAsync<Folder>(folderId) == null)
                throw new FolderNotFoundException(this.DocumentSession.Advanced.GetStringIdFor<Folder>(folderId));

            if (await this._rightsService.HasCurrentUserTheUserId(userId) == false)
                throw new UnauthorizedException();

            if (await this._rightsService.CanCurrentUserAccessFolder(folderId) == false)
                throw new UnauthorizedException();

            string folderStringId = this.DocumentSession.Advanced.GetStringIdFor<Folder>(folderId);
            var query = this.DocumentSession.Query<Note, NotesBySearchTextAndFolderId>().Where(f => f.FolderId == folderStringId);

            List<Note> result = new List<Note>();
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
        /// Gets all notes.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="searchText">The search text.</param>
        [Route("Notes")]
        [RequiresAuthorization]
        public async Task<HttpResponseMessage> GetAllNotes(int userId, string searchText)
        {
            if (await this._rightsService.HasCurrentUserTheUserId(userId) == false)
                throw new UnauthorizedException();

            var query = this.DocumentSession.Query<NotesBySearchTextAndFolderId.Result, NotesBySearchTextAndFolderId>()
                                            .Search(f => f.SearchText, searchText)
                                            .As<Note>();

            List<Note> result = new List<Note>();
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
        /// Creates a new <see cref="Note" />.
        /// </summary>
        /// <param name="note">The note.</param>
        /// <param name="userId">The user id.</param>
        [Route("Notes")]
        [RequiresAuthorization]
        public async Task<HttpResponseMessage> PostNote(Note note, int userId)
        {
            if (note == null)
                throw new InvalidRequestException();

            if (string.IsNullOrWhiteSpace(note.Name))
                throw new InvalidNoteNameException();

            if (await this._rightsService.HasCurrentUserTheUserId(userId) == false)
                throw new UnauthorizedException();

            Folder folder = await this.DocumentSession.LoadAsync<Folder>(note.FolderId);
            if (folder == null)
                throw new FolderNotFoundException(note.FolderId);

            if (folder.UserId != this.DocumentSession.Advanced.GetStringIdFor<User>(userId))
                throw new FolderNotFoundException(this.DocumentSession.Advanced.GetIntIdFrom(folder.Id));

            note.FolderId = folder.Id;
            note.UserId = this.DocumentSession.Advanced.GetStringIdFor<User>(userId);

            await this.DocumentSession.StoreAsync(note);

            this.DocumentSession.Advanced.AddCascadeDelete(folder, note.Id);

            this.Logger.DebugFormat("Created note '{0}' for user '{1}'.", note.Id, note.UserId);

            return Request.CreateResponse(HttpStatusCode.Created, note);
        }

        /// <summary>
        /// Updates the <see cref="Note"/>.
        /// </summary>
        /// <param name="note">The note.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="noteId">The note id.</param>
        [Route("Notes/{noteId:int}")]
        [RequiresAuthorization]
        public async Task<HttpResponseMessage> PutNote(Note note, int userId, int noteId)
        {
            if (note == null)
                throw new InvalidRequestException();

            if (string.IsNullOrWhiteSpace(note.Name))
                throw new InvalidNoteNameException();

            if (await this._rightsService.HasCurrentUserTheUserId(userId) == false)
                throw new UnauthorizedException();

            if (await this._rightsService.CanCurrentUserAccessFolder(note.FolderId) == false)
                throw new UnauthorizedException();

            if (await this._rightsService.CanCurrentUserAccessNote(noteId) == false)
                throw new UnauthorizedException();

            Note storedNote = await this.DocumentSession.LoadAsync<Note>(noteId);

            storedNote.Name = note.Name;
            storedNote.Content = note.Content;
            storedNote.Tags = note.Tags;

            bool folderHasChanged = storedNote.FolderId != note.FolderId;
            if (folderHasChanged)
            {
                Folder oldFolder = await this.DocumentSession.LoadAsync<Folder>(storedNote.FolderId);
                this.DocumentSession.Advanced.RemoveCascadeDelete(oldFolder, storedNote.Id);

                Folder newFolder = await this.DocumentSession.LoadAsync<Folder>(note.FolderId);
                this.DocumentSession.Advanced.AddCascadeDelete(newFolder, storedNote.Id);

                storedNote.FolderId = note.FolderId;
            }

            this.Logger.DebugFormat("Updated note '{0}'.", storedNote.Id);

            return Request.CreateResponse(HttpStatusCode.OK, storedNote);
        }
        /// <summary>
        /// Deletes the <see cref="Note"/>.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="noteId">The note id.</param>
        [Route("Notes/{noteId:int}")]
        [RequiresAuthorization]
        public async Task<HttpResponseMessage> DeleteNote(int userId, int noteId)
        {
            if (await this._rightsService.HasCurrentUserTheUserId(userId) == false)
                throw new UnauthorizedException();

            if (await this._rightsService.CanCurrentUserAccessNote(noteId) == false)
                throw new UnauthorizedException();

            Note note = await this.DocumentSession
                .Include<Note>(f => f.FolderId)
                .LoadAsync<Note>(noteId);
            Folder folder = await this.DocumentSession.LoadAsync<Folder>(note.FolderId);

            this.DocumentSession.Advanced.RemoveCascadeDelete(folder, note.Id);

            this.DocumentSession.Delete(note);

            this.Logger.DebugFormat("Deleted note '{0}' from user '{1}'.", note.Id, note.UserId);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
        #endregion
    }
}
