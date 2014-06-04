using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Caliburn.Micro;
using Newtonsoft.Json.Linq;
using Xemio.SmartNotes.Client.Shared.Clients;
using Xemio.SmartNotes.Client.Windows.Data.Events;
using Xemio.SmartNotes.Client.Windows.Data.Exceptions;
using Xemio.SmartNotes.Shared.Entities.Notes;
using Xemio.SmartNotes.Shared.Helpers;
using Xemio.SmartNotes.Shared.Models;

namespace Xemio.SmartNotes.Client.Windows.Implementations.Tasks
{
    public class MoveNoteTask : BaseTask
    {
        #region Fields
        private readonly IEventAggregator _eventAggregator;
        private readonly WebServiceClient _client;

        private string _noteId;
        private string _newFolderId;
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
        /// Gets or sets the note identifier.
        /// </summary>
        public string NoteId
        {
            get { return this._noteId; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("The NoteId is null or whitespace.", "value");

                this._noteId = value;
            }
        }
        /// <summary>
        /// Gets or sets the name of the note.
        /// </summary>
        public string NoteName { get; set; }
        /// <summary>
        /// Gets or sets the new folder identifier.
        /// </summary>
        public string NewFolderId
        {
            get { return this._newFolderId; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("The NewFolderId is null or whitespace.", "value");

                this._newFolderId = value;
            }
        }
        #endregion

        #region Overrides of BaseTask
        /// <summary>
        /// Gets the display name for this task.
        /// </summary>
        public override string DisplayName
        {
            get { return string.Format(TaskMessages.MoveNoteTask, this.NoteName); }
        }
        /// <summary>
        /// Executes this task.
        /// </summary>
        public override async Task Execute()
        {
            var data = new JObject
            {
                {ReflectionHelper.GetProperty<Note>(f => f.FolderId).Name, this.NewFolderId}
            };

            HttpResponseMessage response = await this._client.Notes.PatchNote(this.NoteId, data);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Note note = await response.Content.ReadAsAsync<Note>();
                this._eventAggregator.PublishOnUIThread(new NoteMovedEvent(note));
            }
            else
            {
                var error = await response.Content.ReadAsAsync<HttpError>();
                this.Logger.Error(error.Message);

                throw new GenericException(TaskMessages.MoveNoteTaskFailed);
            }
        }
        #endregion
    }
}
