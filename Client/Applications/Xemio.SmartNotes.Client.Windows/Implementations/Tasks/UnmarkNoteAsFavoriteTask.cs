using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Xemio.SmartNotes.Client.Shared.Clients;
using Xemio.SmartNotes.Client.Shared.Extensions;
using Xemio.SmartNotes.Client.Windows.Data.Events;
using Xemio.SmartNotes.Client.Windows.Data.Exceptions;
using Xemio.SmartNotes.Shared.Entities.Notes;

namespace Xemio.SmartNotes.Client.Windows.Implementations.Tasks
{
    public class UnmarkNoteAsFavoriteTask : BaseTask
    {
         #region Fields
        private readonly IEventAggregator _eventAggregator;
        private readonly WebServiceClient _client;

        private string _noteId;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the note identifier.
        /// </summary>
        public string NoteId
        {
            get { return this._noteId; }
            set
            {
                if (string.IsNullOrWhiteSpace(value) == true)
                    throw new ArgumentNullException();

                this._noteId = value;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="MarkNoteAsFavoriteTask"/> class.
        /// </summary>
        /// <param name="eventAggregator">The event aggregator.</param>
        /// <param name="client">The client.</param>
        public UnmarkNoteAsFavoriteTask(IEventAggregator eventAggregator, WebServiceClient client)
        {
            this._eventAggregator = eventAggregator;
            this._client = client;
        }
        #endregion

        #region Overrides of BaseTask
        /// <summary>
        /// Gets the display name for this task.
        /// </summary>
        public override string DisplayName
        {
            get { return TaskMessages.UnmarkNoteAsFavoriteTask; }
        }
        /// <summary>
        /// Executes this task.
        /// </summary>
        public override async Task Execute()
        {
            HttpResponseMessage response = await this._client.Notes.UnmarkNoteAsFavorite(this.NoteId);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Note note = await response.Content.ReadAsAsync<Note>();
                this._eventAggregator.Publish(new NoteIsFavoriteChangedEvent(note));
            }
            else
            {
                string message = await response.Content.ReadAsStringAsync();
                this.Logger.Error(message);

                throw new GenericException(TaskMessages.UnmarkNoteAsFavoriteTaskFailed);
            }
        }
        #endregion
    }
}
