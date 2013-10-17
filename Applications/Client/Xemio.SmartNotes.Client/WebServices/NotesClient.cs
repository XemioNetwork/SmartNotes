using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Xemio.SmartNotes.Abstractions.Controllers;
using Xemio.SmartNotes.Client.Data;
using Xemio.SmartNotes.Client.Extensions;
using Xemio.SmartNotes.Entities.Notes;

namespace Xemio.SmartNotes.Client.WebServices
{
    public class NotesClient : BaseClient, INotesController
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
        /// <param name="userId">The user id.</param>
        /// <param name="folderId">The note id.</param>
        public async Task<HttpResponseMessage> GetAllNotes(int userId, int folderId)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["folder"] = folderId.ToString();

            this.SetAuthenticationHeader();
            return await this.Client.GetAsync(string.Format("Users/{0}/Notes?{1}", userId, query));
        }
        /// <summary>
        /// Gets all notes.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="searchText">The search text.</param>
        public async Task<HttpResponseMessage> GetAllNotes(int userId, string searchText)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["searchText"] = searchText;

            this.SetAuthenticationHeader();
            return await this.Client.GetAsync(string.Format("Users/{0}/Notes?{1}", userId, query));
        }
        /// <summary>
        /// Creates a new <see cref="Note" />.
        /// </summary>
        /// <param name="note">The note.</param>
        /// <param name="userId">The user id.</param>
        public async Task<HttpResponseMessage> PostNote(Note note, int userId)
        {
            string requestString = await JsonConvert.SerializeObjectAsync(note);

            this.SetAuthenticationHeader(requestString);
            return await this.Client.PostJsonAsync(string.Format("Users/{0}/Notes", userId), requestString);
        }
        /// <summary>
        /// Updates the <see cref="Note"/>.
        /// </summary>
        /// <param name="note">The note.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="noteId">The note id.</param>
        public async Task<HttpResponseMessage> PutNote(Note note, int userId, int noteId)
        {
            string requestString = await JsonConvert.SerializeObjectAsync(note);

            this.SetAuthenticationHeader(requestString);
            return await this.Client.PutJsonAsync(string.Format("Users/{0}/Notes/{1}", userId, noteId), requestString);
        }
        /// <summary>
        /// Deletes the <see cref="Note"/>.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="noteId">The note id.</param>
        public async Task<HttpResponseMessage> DeleteNote(int userId, int noteId)
        {
            this.SetAuthenticationHeader(string.Empty);
            return await this.Client.DeleteAsync(string.Format("Users/{0}/Notes/{1}", userId, noteId));
        }
        #endregion
    }
}
