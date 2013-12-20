using System.Net.Http;
using Xemio.SmartNotes.Models.Entities.Notes;

namespace Xemio.SmartNotes.Server.Abstractions.Controllers
{
    public interface INotesController : IController
    {
        /// <summary>
        /// Returns all <see cref="Note" />s from the given <see cref="Folder" />.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="folderId">The note id.</param>
        HttpResponseMessage GetAllNotes(int userId, int folderId);
        /// <summary>
        /// Gets all notes.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="searchText">The search text.</param>
        HttpResponseMessage GetAllNotes(int userId, string searchText);
        /// <summary>
        /// Creates a new <see cref="Note" />.
        /// </summary>
        /// <param name="note">The note.</param>
        /// <param name="userId">The user id.</param>
        HttpResponseMessage PostNote(Note note, int userId);
        /// <summary>
        /// Updates the <see cref="Note"/>.
        /// </summary>
        /// <param name="note">The note.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="noteId">The note id.</param>
        HttpResponseMessage PutNote(Note note, int userId, int noteId);
        /// <summary>
        /// Deletes the <see cref="Note"/>.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="noteId">The note id.</param>
        HttpResponseMessage DeleteNote(int userId, int noteId);
    }
}