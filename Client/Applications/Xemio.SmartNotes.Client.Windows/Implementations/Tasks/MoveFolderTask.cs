using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Newtonsoft.Json.Linq;
using Xemio.SmartNotes.Client.Shared.Clients;
using Xemio.SmartNotes.Client.Shared.Extensions;
using Xemio.SmartNotes.Client.Windows.Data.Events;
using Xemio.SmartNotes.Client.Windows.Data.Exceptions;
using Xemio.SmartNotes.Shared.Entities.Notes;
using Xemio.SmartNotes.Shared.Helpers;
using Xemio.SmartNotes.Shared.Models;

namespace Xemio.SmartNotes.Client.Windows.Implementations.Tasks
{
    public class MoveFolderTask : BaseTask
    {
        #region Fields
        private readonly IEventAggregator _eventAggregator;
        private readonly WebServiceClient _client;

        private string _folderId;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="MoveFolderTask"/> class.
        /// </summary>
        /// <param name="eventAggregator">The event aggregator.</param>
        /// <param name="client">The client.</param>
        public MoveFolderTask(IEventAggregator eventAggregator, WebServiceClient client)
        {
            this._eventAggregator = eventAggregator;
            this._client = client;
        }
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the folder identifier.
        /// </summary>
        public string FolderId
        {
            get { return this._folderId; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("FolderId is null or whitespace.", "value");

                this._folderId = value;
            }
        }
        /// <summary>
        /// Gets or sets the name of the folder.
        /// </summary>
        public string FolderName { get; set; }
        /// <summary>
        /// Gets or sets the new parent folder identifier.
        /// </summary>
        public string NewParentFolderId { get; set; }
        #endregion

        #region Overrides of BaseTask
        /// <summary>
        /// Gets the display name for this task.
        /// </summary>
        public override string DisplayName
        {
            get { return string.Format(TaskMessages.MoveFolderTask, this.FolderName); }
        }
        /// <summary>
        /// Executes this task.
        /// </summary>
        public override async Task Execute()
        {
            var data = new JObject
            {
                {ReflectionHelper.GetProperty<Folder>(f => f.ParentFolderId).Name, this.NewParentFolderId}
            };

            HttpResponseMessage response = await this._client.Folders.PatchFolder(this.FolderId, data);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Folder folder = await response.Content.ReadAsAsync<Folder>();
                this._eventAggregator.Publish(new FolderMovedEvent(folder));
            }
            else
            {
                var error = await response.Content.ReadAsAsync<Error>();
                this.Logger.Error(error.ToString);

                throw new GenericException(TaskMessages.MoveFolderTaskFailed);
            }
        }
        #endregion
    }
}
