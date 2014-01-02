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
using Xemio.SmartNotes.Server.Infrastructure.Raven.Indexes;

namespace Xemio.SmartNotes.Server.Infrastructure.Controllers
{
    /// <summary>
    /// Controller for the <see cref="Note"/> class.
    /// </summary>
    [RoutePrefix("Users/Authorized")]
    public class NotesController : BaseController
    {
        #region Fields
        private readonly IRightsService _rightsService;
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NotesController"/> class.
        /// </summary>
        /// <param name="documentSession">The document session.</param>
        /// <param name="userService">The user service.</param>
        /// <param name="rightsService">The right service.</param>
        public NotesController(IDocumentSession documentSession, IUserService userService, IRightsService rightsService)
            : base(documentSession, userService)
        {
            this._rightsService = rightsService;
        }
        #endregion

        #region Implementation of INotesController
        /// <summary>
        /// Returns all <see cref="Note" />s from the given <see cref="Folder" />.
        /// </summary>
        /// <param name="folderId">The note id.</param>
        [Route("Notes")]
        [RequiresAuthorization]
        public HttpResponseMessage GetAllNotes(int folderId)
        {
            if (this.DocumentSession.Load<Folder>(folderId) == null)
                throw new FolderNotFoundException(this.DocumentSession.Advanced.GetStringIdFor<Folder>(folderId));
            
            if (this._rightsService.CanCurrentUserAccessFolder(folderId, false) == false)
                throw new UnauthorizedException();

            string folderStringId = this.DocumentSession.Advanced.GetStringIdFor<Folder>(folderId);

            var notes = this.DocumentSession.Query<Note, NotesBySearchTextAndFolderId>()
                                            .Where(f => f.FolderId == folderStringId).ToList();
            
            return Request.CreateResponse(HttpStatusCode.Found, notes);
        }
        /// <summary>
        /// Gets all notes.
        /// </summary>
        /// <param name="searchText">The search text.</param>
        [Route("Notes")]
        [RequiresAuthorization]
        public HttpResponseMessage GetAllNotes(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
                return Request.CreateResponse(HttpStatusCode.NotFound);

            var notes = this.DocumentSession.Query<NotesBySearchTextAndFolderId.Result, NotesBySearchTextAndFolderId>()
                                            .Search(f => f.SearchText, searchText)
                                            .As<Note>()
                                            .ToList();

            return Request.CreateResponse(HttpStatusCode.Found, notes);
        }
        /// <summary>
        /// Creates a new <see cref="Note" />.
        /// </summary>
        /// <param name="note">The note.</param>
        [Route("Notes")]
        [RequiresAuthorization]
        public HttpResponseMessage PostNote(Note note)
        {
            if (note == null)
                throw new InvalidRequestException();

            if (string.IsNullOrWhiteSpace(note.Name))
                throw new InvalidNoteNameException();
            
            if (this._rightsService.CanCurrentUserAccessFolder(note.FolderId, false) == false)
                throw new UnauthorizedException();

            var currentUser = this.UserService.GetCurrentUser();
            var folder = this.DocumentSession.Load<Folder>(note.FolderId);

            note.UserId = currentUser.Id;

            this.DocumentSession.Store(note);
            this.DocumentSession.Advanced.AddCascadeDelete(folder, note.Id);

            this.Logger.DebugFormat("Created note '{0}' for user '{1}'.", note.Id, note.UserId);

            return Request.CreateResponse(HttpStatusCode.Created, note);
        }

        /// <summary>
        /// Updates the <see cref="Note"/>.
        /// </summary>
        /// <param name="note">The note.</param>
        /// <param name="noteId">The note id.</param>
        [Route("Notes/{noteId:int}")]
        [RequiresAuthorization]
        public HttpResponseMessage PutNote(Note note, int noteId)
        {
            if (note == null)
                throw new InvalidRequestException();

            if (string.IsNullOrWhiteSpace(note.Name))
                throw new InvalidNoteNameException();
            
            if (this._rightsService.CanCurrentUserAccessFolder(note.FolderId, false) == false)
                throw new UnauthorizedException();

            if (this._rightsService.CanCurrentUserAccessNote(noteId, false) == false)
                throw new UnauthorizedException();

            var storedNote = this.DocumentSession.Load<Note>(noteId);

            storedNote.Name = note.Name;
            storedNote.Content = note.Content;
            storedNote.Tags = note.Tags;

            bool folderHasChanged = storedNote.FolderId != note.FolderId;
            if (folderHasChanged)
            {
                var oldFolder = this.DocumentSession.Load<Folder>(storedNote.FolderId);
                this.DocumentSession.Advanced.RemoveCascadeDelete(oldFolder, storedNote.Id);

                var newFolder = this.DocumentSession.Load<Folder>(note.FolderId);
                this.DocumentSession.Advanced.AddCascadeDelete(newFolder, storedNote.Id);

                storedNote.FolderId = note.FolderId;
            }

            this.Logger.DebugFormat("Updated note '{0}'.", storedNote.Id);

            return Request.CreateResponse(HttpStatusCode.OK, storedNote);
        }
        /// <summary>
        /// Deletes the <see cref="Note"/>.
        /// </summary>
        /// <param name="noteId">The note id.</param>
        [Route("Notes/{noteId:int}")]
        [RequiresAuthorization]
        public HttpResponseMessage DeleteNote(int noteId)
        {
            if (this._rightsService.CanCurrentUserAccessNote(noteId, false) == false)
                throw new UnauthorizedException();

            var note = this.DocumentSession
                           .Include<Note>(f => f.FolderId)
                           .Load<Note>(noteId);

            var folder = this.DocumentSession.Load<Folder>(note.FolderId);

            this.DocumentSession.Advanced.RemoveCascadeDelete(folder, note.Id);
            this.DocumentSession.Delete(note);

            this.Logger.DebugFormat("Deleted note '{0}' from user '{1}'.", note.Id, note.UserId);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
        #endregion
    }
}
