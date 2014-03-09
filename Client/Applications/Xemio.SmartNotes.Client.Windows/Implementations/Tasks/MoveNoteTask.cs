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
    public class MoveNoteTask : BaseTask
    {
        #region Fields
        private readonly IEventAggregator _eventAggregator;
        private readonly WebServiceClient _client;

        private Note _note;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="MoveNoteTask"/> class.
        /// </summary>
        /// <param name="eventAggregator">The event aggregator.</param>
        /// <param name="client">The client.</param>
        public MoveNoteTask(IEventAggregator eventAggregator, WebServiceClient client)
        {
            this._eventAggregator = eventAggregator;
            this._client = client;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the note.
        /// </summary>
        public Note Note
        {
            get { return this._note; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                this._note = value;
            }
        }
        #endregion

        #region Overrides of BaseTask
        /// <summary>
        /// Gets the display name for this task.
        /// </summary>
        public override string DisplayName
        {
            get { return string.Format(TaskMessages.MoveNoteTask, this.Note.Name); }
        }
        /// <summary>
        /// Executes this task.
        /// </summary>
        public override async Task Execute()
        {
            HttpResponseMessage response = await this._client.Notes.PutNote(this.Note);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Note note = await response.Content.ReadAsAsync<Note>();
                this._eventAggregator.Publish(new NoteMovedEvent(note));
            }
            else
            {
                string message = await response.Content.ReadAsStringAsync();
                this.Logger.Error(message);

                throw new GenericException(TaskMessages.MoveNoteTaskFailed);
            }
        }
        #endregion
    }
}
