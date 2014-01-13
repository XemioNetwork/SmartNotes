using System.Collections.Specialized;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Xemio.SmartNotes.Abstractions.Extensions;
using Xemio.SmartNotes.Client.Abstractions.Clients;
using Xemio.SmartNotes.Models.Entities.Notes;
using Xemio.SmartNotes.Models.Entities.Users;

namespace Xemio.SmartNotes.Client.Shared.WebService
{
    public class NotesClient : BaseClient, INotesClient
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NotesClient"/> class.
        /// </summary>
        /// <param name="baseAddress">The base address.</param>
        /// <param name="session">The application session. </param>
        public NotesClient(string baseAddress, Session session)
            : base(baseAddress, session)
        {
        }
        #endregion

        #region Implementation of INotesController
        /// <summary>
        /// Returns all <see cref="Note" />s from the given <see cref="Folder" />.
        /// </summary>
        /// <param name="folderId">The note id.</param>
        public Task<HttpResponseMessage> GetAllNotes(string folderId)
        {
            var query = new HttpQueryBuilder();
            query.AddParameter("folder", folderId.GetIntId());

            var request = this.CreateRequest(HttpMethod.Get, string.Format("Users/Authorized/Notes{0}", query));
            return this.Client.SendAsync(request);
        }
        /// <summary>
        /// Gets all notes.
        /// </summary>
        /// <param name="searchText">The search text.</param>
        public Task<HttpResponseMessage> SearchNotes(string searchText)
        {
            var query = new HttpQueryBuilder();
            query.AddParameter("searchText", searchText);

            var request = this.CreateRequest(HttpMethod.Get, string.Format("Users/Authorized/Notes{0}", query));
            return this.Client.SendAsync(request);
        }
        /// <summary>
        /// Creates a new <see cref="Note" />.
        /// </summary>
        /// <param name="note">The note.</param>
        public Task<HttpResponseMessage> PostNote(Note note)
        {
            var request = this.CreateRequest(HttpMethod.Post, "Users/Authorized/Notes", note);
            return this.Client.SendAsync(request);
        }
        /// <summary>
        /// Updates the <see cref="Note"/>.
        /// </summary>
        /// <param name="note">The note.</param>
        public Task<HttpResponseMessage> PutNote(Note note)
        {
            var request = this.CreateRequest(HttpMethod.Put, string.Format("Users/Authorized/Notes/{0}", note.Id.GetIntId()), note);
            return this.Client.SendAsync(request);
        }
        /// <summary>
        /// Deletes the <see cref="Note"/>.
        /// </summary>
        /// <param name="noteId">The note id.</param>
        public Task<HttpResponseMessage> DeleteNote(string noteId)
        {
            var request = this.CreateRequest(HttpMethod.Delete, string.Format("Users/Authorized/Notes/{0}", noteId.GetIntId()));
            return this.Client.SendAsync(request);
        }
        #endregion
    }
}
