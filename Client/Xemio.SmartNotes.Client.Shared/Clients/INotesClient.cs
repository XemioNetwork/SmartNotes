using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Xemio.SmartNotes.Shared.Entities.Notes;

namespace Xemio.SmartNotes.Client.Shared.Clients
{
    public interface INotesClient : IClient
    {
        /// <summary>
        /// Returns all <see cref="Note" />s from the given <see cref="Folder" />.
        /// </summary>
        /// <param name="folderId">The note id.</param>
        Task<HttpResponseMessage> GetAllNotes(string folderId);

        /// <summary>
        /// Gets all notes.
        /// </summary>
        /// <param name="searchText">The search text.</param>
        Task<HttpResponseMessage> SearchNotes(string searchText);

        /// <summary>
        /// Gets the favorite notes.
        /// </summary>
        Task<HttpResponseMessage> GetFavoriteNotes();

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
        Task<HttpResponseMessage> DeleteNote(string noteId);

        /// <summary>
        /// Patches the <see cref="Note"/>.
        /// </summary>
        /// <param name="noteId">The note identifier.</param>
        /// <param name="data">The data.</param>
        Task<HttpResponseMessage> PatchNote(string noteId, JObject data);
    }
}