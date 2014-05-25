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
using Xemio.SmartNotes.Shared.Models;

namespace Xemio.SmartNotes.Client.Windows.Implementations.Tasks
{
    public class DeleteFolderTask : BaseTask
    {
        #region Fields
        private readonly WebServiceClient _client;
        private readonly IEventAggregator _eventAggregator;

        private string _folderName;
        private string _folderId;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the name of the folder.
        /// </summary>
        public string FolderName
        {
            get { return this._folderName; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                this._folderName = value;
            }
        }
        /// <summary>
        /// Gets or sets the folder identifier.
        /// </summary>
        public string FolderId
        {
            get { return _folderId; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException("value");

                this._folderId = value;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteFolderTask"/> class.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="eventAggregator">The event aggregator.</param>
        public DeleteFolderTask(WebServiceClient client, IEventAggregator eventAggregator)
        {
            this._client = client;
            this._eventAggregator = eventAggregator;
        }
        #endregion

        #region Overrides of BaseTask
        /// <summary>
        /// Gets the display name for this task.
        /// </summary>
        public override string DisplayName
        {
            get { return string.Format(TaskMessages.DeleteFolderTask, this.FolderName); }
        }
        /// <summary>
        /// Executes this task.
        /// </summary>
        public override async Task Execute()
        {
            HttpResponseMessage response = await this._client.Folders.DeleteFolder(this.FolderId);
            if (response.StatusCode == HttpStatusCode.OK)
            { 
                this._eventAggregator.Publish(new FolderDeletedEvent(this.FolderId));
            }
            else
            {
                var error = await response.Content.ReadAsAsync<Error>();
                this.Logger.ErrorFormat("Error while deleting folder '{0}': {1}.", this.FolderId, error);

                throw new GenericException(TaskMessages.DeleteFolderTaskFailed);
            }
        }
        #endregion
    }
}
