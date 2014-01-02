using System.Net.Http;
using System.Threading.Tasks;
using Xemio.SmartNotes.Models.Entities.Notes;

namespace Xemio.SmartNotes.Client.Abstractions.Clients
{
    public interface INotesClient : IClient
    {
        /// <summary>
        /// Returns all <see cref="Note" />s from the given <see cref="Folder" />.
        /// </summary>
        /// <param name="folderId">The note id.</param>
        Task<HttpResponseMessage> GetAllNotes(int folderId);

        /// <summary>
        /// Gets all notes.
        /// </summary>
        /// <param name="searchText">The search text.</param>
        Task<HttpResponseMessage> GetAllNotes(string searchText);

        /// <summary>
        /// Creates a new <see cref="Note" />.
        /// </summary>
        /// <param name="note">The note.</param>
        Task<HttpResponseMessage> PostNote(Note note);

        /// <summary>
        /// Updates the <see cref="Note"/>.
        /// </summary>
        /// <param name="note">The note.</param>
        Task<HttpResponseMessage> PutNote(Note note);

        /// <summary>
        /// Deletes the <see cref="Note"/>.
        /// </summary>
        /// <param name="noteId">The note id.</param>
        Task<HttpResponseMessage> DeleteNote(int noteId);
    }
}