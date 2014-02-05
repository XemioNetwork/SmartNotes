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
    public class EditFolderTask : BaseTask
    {
        #region Fields
        private readonly IEventAggregator _eventAggregator;
        private readonly WebServiceClient _client;

        private Folder _folder;

        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="EditFolderTask"/> class.
        /// </summary>
        /// <param name="eventAggregator">The event aggregator.</param>
        /// <param name="client">The client.</param>
        public EditFolderTask(IEventAggregator eventAggregator, WebServiceClient client)
        {
            this._eventAggregator = eventAggregator;
            this._client = client;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the folder that will be updated.
        /// </summary>
        public Folder Folder
        {
            get { return this._folder; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                this._folder = value;
            }
        }
        #endregion

        #region Implementation of ITask
        /// <summary>
        /// Gets the display name for this task.
        /// </summary>
        public override string DisplayName
        {
            get { return string.Format(TaskMessages.EditFolderTask, this.Folder.Name); }
        }

        public override async Task Execute()
        {
            HttpResponseMessage response = await this._client.Folders.PutFolder(this.Folder);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Folder folder = await response.Content.ReadAsAsync<Folder>();
                this._eventAggregator.Publish(new FolderEditedEvent(folder));
            }
            else
            {
                string message = await response.Content.ReadAsStringAsync();
                this.Logger.Error(message);

                throw new GenericException(TaskMessages.EditFolderTaskFailed);
            }
        }
        #endregion
    }
}
