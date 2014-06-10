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
    public class EditFolderTask : BaseTask
    {
        #region Internal
        public class DisplayData
        {
            public string FolderName { get; set; }
        }
        #endregion

        #region Fields
        private readonly IEventAggregator _eventAggregator;
        private readonly WebServiceClient _client;

        private string _folderId;
        private string _newFolderName;
        private string[] _newFolderTags;
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

            this.Display = new DisplayData();
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
                    throw new ArgumentNullException("value");

                this._folderId = value;
            }
        }
        /// <summary>
        /// Gets or sets the new name of the folder.
        /// </summary>
        public string NewFolderName
        {
            get { return this._newFolderName; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException("value");
                
                this._newFolderName = value;
            }
        }
        /// <summary>
        /// Gets or sets the new folder tags.
        /// </summary>
        public string[] NewFolderTags
        {
            get { return this._newFolderTags; }
            set
            {
                if (value == null || value.Length == 0)
                    throw new ArgumentNullException("value");

                this._newFolderTags = value;
            }
        }
        /// <summary>
        /// Gets the display data.
        /// </summary>
        public DisplayData Display { get; private set; }
        #endregion

        #region Implementation of ITask
        /// <summary>
        /// Gets the display name for this task.
        /// </summary>
        public override string DisplayName
        {
            get { return string.Format(TaskMessages.EditFolderTask, this.Display.FolderName); }
        }
        /// <summary>
        /// Executes this task.
        /// </summary>
        public override async Task Execute()
        {
            var data = new JObject
            {
                {ReflectionHelper.GetProperty<Note>(f => f.Name).Name, this.NewFolderName},
                {ReflectionHelper.GetProperty<Note>(f => f.Tags).Name, new JArray(this.NewFolderTags)}
            };

            HttpResponseMessage response = await this._client.Folders.PatchFolder(this.FolderId, data);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Folder folder = await response.Content.ReadAsAsync<Folder>();
                this._eventAggregator.PublishOnUIThread(new FolderEditedEvent(folder));
            }
            else
            {
                var error = await response.Content.ReadAsAsync<HttpError>();
                this.Logger.Error(error.Message);

                throw new GenericException(TaskMessages.EditFolderTaskFailed);
            }
        }
        #endregion
    }
}
