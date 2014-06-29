﻿using System;
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
    public class MoveFolderTask : BaseTask
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
                    throw new ArgumentException("value");

                this._folderId = value;
            }
        }
        /// <summary>
        /// Gets or sets the new parent folder identifier.
        /// </summary>
        public string NewParentFolderId { get; set; }
        /// <summary>
        /// Gets the display data.
        /// </summary>
        public DisplayData Display { get; private set; }
        #endregion

        #region Overrides of BaseTask
        /// <summary>
        /// Gets the display name for this task.
        /// </summary>
        public override string DisplayName
        {
            get { return string.Format(TaskMessages.MoveFolderTask, this.Display.FolderName); }
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
                this._eventAggregator.PublishOnUIThread(new FolderEditedEvent(folder));
            }
            else
            {
                var error = await response.Content.ReadAsAsync<HttpError>();
                this.Logger.Error(error.Message);

                throw new TaskException(TaskMessages.MoveFolderTaskFailed);
            }
        }
        #endregion
    }
}
