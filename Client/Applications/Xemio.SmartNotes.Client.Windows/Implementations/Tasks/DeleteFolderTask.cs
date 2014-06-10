using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Caliburn.Micro;
using Xemio.SmartNotes.Client.Shared.Clients;
using Xemio.SmartNotes.Client.Windows.Data.Events;
using Xemio.SmartNotes.Client.Windows.Data.Exceptions;
using Xemio.SmartNotes.Shared.Models;

namespace Xemio.SmartNotes.Client.Windows.Implementations.Tasks
{
    public class DeleteFolderTask : BaseTask
    {
        #region Internal
        public class DisplayData
        {
            public string FolderName { get; set; }
        }
        #endregion

        #region Fields
        private readonly WebServiceClient _client;
        private readonly IEventAggregator _eventAggregator;

        private string _folderName;
        private string _folderId;
        #endregion

        #region Properties
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
        /// <summary>
        /// Gets the display data.
        /// </summary>
        public DisplayData Display { get; private set; }
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

            this.Display = new DisplayData();
        }
        #endregion

        #region Overrides of BaseTask
        /// <summary>
        /// Gets the display name for this task.
        /// </summary>
        public override string DisplayName
        {
            get { return string.Format(TaskMessages.DeleteFolderTask, this.Display.FolderName); }
        }
        /// <summary>
        /// Executes this task.
        /// </summary>
        public override async Task Execute()
        {
            HttpResponseMessage response = await this._client.Folders.DeleteFolder(this.FolderId);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                this._eventAggregator.PublishOnUIThread(new FolderDeletedEvent(this.FolderId));
            }
            else
            {
                var error = await response.Content.ReadAsAsync<HttpError>();
                this.Logger.ErrorFormat("Error while deleting folder '{0}': {1}.", this.FolderId, error.Message);

                throw new GenericException(TaskMessages.DeleteFolderTaskFailed);
            }
        }
        #endregion
    }
}
