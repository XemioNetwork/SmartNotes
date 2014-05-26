using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Xemio.SmartNotes.Shared.Entities.Notes;
using Xemio.SmartNotes.Shared.Models;

namespace Xemio.SmartNotes.Client.Windows.Implementations.Tasks
{
    public class CreateFolderTask : BaseTask
    {
        #region Fields
        private readonly IEventAggregator _eventAggregator;
        private readonly WebServiceClient _client;

        private string _folderName;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the name of the new folder.
        /// </summary>
        public string FolderName
        {
            get { return this._folderName; }
            set
            {
                if (string.IsNullOrWhiteSpace(value) == true)
                    throw new ArgumentNullException();

                this._folderName = value;
            }
        }
        /// <summary>
        /// Gets or sets the tags of the new folder.
        /// </summary>
        public string[] FolderTags { get; set; }
        /// <summary>
        /// Gets or sets the parent folder identifier.
        /// </summary>
        public string ParentFolderId { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateFolderTask"/> class.
        /// </summary>
        /// <param name="eventAggregator">The event aggregator.</param>
        /// <param name="client">The webservice client.</param>
        public CreateFolderTask(IEventAggregator eventAggregator, WebServiceClient client)
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
            get { return TaskMessages.CreateFolderTask; }
        }
        /// <summary>
        /// Executes this task.
        /// </summary>
        public override async Task Execute()
        {
            var newFolder = new Folder
            {
                Name = this.FolderName,
                ParentFolderId = this.ParentFolderId,
                Tags = new Collection<string>(this.FolderTags)
            };

            HttpResponseMessage response = await this._client.Folders.PostFolder(newFolder);
            if (response.StatusCode == HttpStatusCode.Created)
            {
                Folder createdFolder = await response.Content.ReadAsAsync<Folder>();
                this._eventAggregator.Publish(new FolderCreatedEvent(createdFolder));
            }
            else
            {
                var error = await response.Content.ReadAsAsync<HttpError>();
                this.Logger.Error(error.Message);

                throw new GenericException(TaskMessages.CreateFolderTaskFailed);
            }
        }
        #endregion
    }
}
