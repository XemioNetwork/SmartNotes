using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Windows;
using Caliburn.Micro;
using Castle.Core.Logging;
using Xemio.SmartNotes.Client.Shared.Clients;
using Xemio.SmartNotes.Client.Shared.Tasks;
using Xemio.SmartNotes.Client.Windows.Extensions;
using Xemio.SmartNotes.Client.Windows.Implementations.Interaction;
using Xemio.SmartNotes.Client.Windows.Implementations.Tasks;
using Xemio.SmartNotes.Client.Windows.Views.CreateFolder;
using Xemio.SmartNotes.Shared.Entities.Notes;
using Xemio.SmartNotes.Shared.Models;

namespace Xemio.SmartNotes.Client.Windows.Views.EditFolder
{
    public class EditFolderViewModel : Screen
    {
        #region Fields
        private readonly ITaskExecutor _taskExecutor;
        private readonly WebServiceClient _client;
        private readonly DisplayManager _displayManager;

        private string _folderName;
        private string _folderTags;
        private string _exampleTags;
        private Folder _folder;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="EditFolderViewModel"/> class.
        /// </summary>
        /// <param name="taskExecutor">The task executor.</param>
        /// <param name="client">The client.</param>
        /// <param name="displayManager">The display manager.</param>
        public EditFolderViewModel(ITaskExecutor taskExecutor, WebServiceClient client, DisplayManager displayManager)
        {
            this.Logger = NullLogger.Instance;

            this._taskExecutor = taskExecutor;
            this._client = client;
            this._displayManager = displayManager;

            this.DisplayName = "Xemio Notes";
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        public ILogger Logger { get; set; }
        /// <summary>
        /// Gets or sets the folder.
        /// </summary>
        public Folder Folder
        {
            get { return this._folder; }
            set
            {
                if (this._folder != value)
                { 
                    this._folder = value;

                    this.FolderName = this.Folder.Name;
                    this.FolderTags = string.Join(", ", this.Folder.Tags);

                    this.NotifyOfPropertyChange(() => this.Folder);
                }
            }
        }
        /// <summary>
        /// Gets or sets the name of the folder.
        /// </summary>
        public string FolderName
        {
            get { return this._folderName; }
            set
            {
                if (this._folderName != value)
                {
                    this._folderName = value;
                    this.NotifyOfPropertyChange(() => this.FolderName);
                    this.NotifyOfPropertyChange(() => this.CanEditFolder);
                }
            }
        }
        /// <summary>
        /// Gets or sets the tags of the folder.
        /// </summary>
        public string FolderTags
        {
            get { return this._folderTags; }
            set
            {
                if (this._folderTags != value)
                {
                    this._folderTags = value;
                    this.NotifyOfPropertyChange(() => this.FolderTags);
                }
            }
        }
        /// <summary>
        /// Gets or sets the example tags.
        /// </summary>
        public string ExampleTags
        {
            get { return this._exampleTags; }
            set
            {
                if (this._exampleTags != value)
                {
                    this._exampleTags = value;
                    this.NotifyOfPropertyChange(() => this.ExampleTags);
                }
            }
        }
        #endregion

        #region Overrides of Screen
        /// <summary>
        /// Called when initializing.
        /// </summary>
        protected override async void OnInitialize()
        {
            await this.LoadTags();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Gets a value indicating whether you can create the folder.
        /// </summary>
        public bool CanEditFolder
        {
            get { return string.IsNullOrWhiteSpace(this.FolderName) == false; }
        }
        /// <summary>
        /// Creates the folder.
        /// </summary>
        public void EditFolder()
        {
            var task = IoC.Get<EditFolderTask>();
            task.FolderId = this.Folder.Id;
            task.NewFolderName = this.FolderName;
            task.NewFolderTags = this.FolderTags.GetTags();
            task.Display.FolderName = this.Folder.Name;

            this._taskExecutor.StartTask(task);

            this.TryClose();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Loads the commonly used tags.
        /// </summary>
        private async Task LoadTags()
        {
            HttpResponseMessage response = await this._client.Tags.GetTags(5);
            if (response.StatusCode == HttpStatusCode.Found)
            {
                Tag[] tags = await response.Content.ReadAsAsync<Tag[]>();
                if (tags.Any())
                {
                    this.ExampleTags = string.Format("{0} {1}", CreateFolderMessages.ForExample, string.Join(", ", tags.Select(f => f.Name)));
                }
            }
            else
            {
                var error = await response.Content.ReadAsAsync<HttpError>();
                this.Logger.ErrorFormat("Error while loading the tags: {0}", error.Message);

                this._displayManager.Messages.ShowMessageBox(error.Message, ClientMessages.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion
    }
}
