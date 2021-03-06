﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Castle.Core.Internal;
using Raven.Abstractions.Data;
using Raven.Client;
using Xemio.SmartNotes.Server.Abstractions.Services;
using Xemio.SmartNotes.Server.Infrastructure.Deltas;
using Xemio.SmartNotes.Server.Infrastructure.Exceptions;
using Xemio.SmartNotes.Server.Infrastructure.Extensions;
using Xemio.SmartNotes.Server.Infrastructure.Filters;
using Xemio.SmartNotes.Server.Infrastructure.RavenDB.Indexes;
using Xemio.SmartNotes.Shared.Entities.Notes;
using Xemio.SmartNotes.Shared.Entities.Users;

namespace Xemio.SmartNotes.Server.Infrastructure.Controllers
{
    /// <summary>
    /// Controller for the <see cref="Note"/> class.
    /// </summary>
    [RoutePrefix("Users/Me")]
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

        #region Methods
        /// <summary>
        /// Returns all favorite <see cref="Note"/>s.
        /// </summary>
        [Route("Notes/Favorites")]
        [RequiresAuthorization]
        public HttpResponseMessage GetAllFavoriteNotes()
        {
            User currentUser = this.UserService.GetCurrentUser();

            var notes = this.DocumentSession.Query<NotesBySearchTextAndOthers.Result, NotesBySearchTextAndOthers>()
                                            .Where(f => f.UserId == currentUser.Id && f.IsFavorite)
                                            .OrderByDescending(f => f.CreatedDate)
                                            .As<Note>()
                                            .ToList();

            return Request.CreateResponse(HttpStatusCode.Found, notes);
        }
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

            var notes = this.DocumentSession.Query<NotesBySearchTextAndOthers.Result, NotesBySearchTextAndOthers>()
                                            .Where(f => f.FolderId == folderStringId)
                                            .OrderByDescending(f => f.CreatedDate)
                                            .As<Note>()
                                            .ToList();
            
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

            User currentUser = this.UserService.GetCurrentUser();

            var notes = this.DocumentSession.Query<NotesBySearchTextAndOthers.Result, NotesBySearchTextAndOthers>()
                                            .Where(f => f.UserId == currentUser.Id)
                                            .Search(f => f.SearchText, searchText, options: SearchOptions.And)
                                            .As<Note>()
                                            .OrderByDescending(f => f.CreatedDate)
                                            .ToList();
            
            if (notes.Count > 0)
                return Request.CreateResponse(HttpStatusCode.Found, notes);

            var suggestQuery = new SuggestionQuery
            {
                Field = "SearchText",
                Popularity = true,
            };

            SuggestionQueryResult suggestionResult = this.DocumentSession.Query<NotesBySearchTextAndUserIdForSuggestions.Result, NotesBySearchTextAndUserIdForSuggestions>()
                                                                         .Where(f => f.UserId == currentUser.Id)
                                                                         .Search(f => f.SearchText, searchText, options: SearchOptions.And)
                                                                         .Suggest(suggestQuery);

            if (suggestionResult.Suggestions.Length > 0)
                return Request.CreateResponse(HttpStatusCode.SeeOther, suggestionResult.Suggestions);

            return Request.CreateResponse(HttpStatusCode.NotFound);
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

            note.UserId = currentUser.Id;
            note.CreatedDate = DateTimeOffset.Now;

            this.DocumentSession.Store(note);

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
            storedNote.FolderId = note.FolderId;
            storedNote.IsFavorite = note.IsFavorite;

            this.Logger.DebugFormat("Updated note '{0}'.", storedNote.Id);

            return Request.CreateResponse(HttpStatusCode.OK, storedNote);
        }
        /// <summary>
        /// Patches the <see cref="Note"/>.
        /// </summary>
        /// <param name="note">The note.</param>
        /// <param name="noteId">The note identifier.</param>
        [Route("Notes/{noteId:int}")]
        [RequiresAuthorization]
        public HttpResponseMessage PatchNote(Delta<Note> note, int noteId)
        {
            if (note == null)
                throw new InvalidRequestException();

            if (this._rightsService.CanCurrentUserAccessNote(noteId, false) == false)
                throw new UnauthorizedException();

            if (note.HasPropertyChanged(f => f.FolderId) &&
                this._rightsService.CanCurrentUserAccessFolder(note.GetPropertyValue(f => f.FolderId), false) == false)
                throw new UnauthorizedException();
            
            if (note.HasPropertyChanged(f => f.Name) &&
                string.IsNullOrWhiteSpace(note.GetPropertyValue(f => f.Name)))
                throw new InvalidNoteNameException();

            if (note.HasPropertyChanged(f => f.UserId))
                throw new InvalidRequestException();

            if (note.HasPropertyChanged(f => f.Id))
                throw new InvalidRequestException();

            if (note.HasPropertyChanged(f => f.CreatedDate))
                throw new InvalidRequestException();

            var storedNote = this.DocumentSession.Load<Note>(noteId);
            note.Patch(storedNote);

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
                           .Load<Note>(noteId);

            this.DocumentSession.Delete(note);

            this.Logger.DebugFormat("Deleted note '{0}' from user '{1}'.", note.Id, note.UserId);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
        #endregion
    }
}
