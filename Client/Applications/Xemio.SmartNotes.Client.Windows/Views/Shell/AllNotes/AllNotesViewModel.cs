using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using Castle.Core.Logging;
using Xemio.CommonLibrary.Security;
using Xemio.SmartNotes.Client.Shared.Clients;
using Xemio.SmartNotes.Client.Shared.Extensions;
using Xemio.SmartNotes.Client.Shared.Tasks;
using Xemio.SmartNotes.Client.Windows.Data.Events;
using Xemio.SmartNotes.Client.Windows.Implementations.Interaction;
using Xemio.SmartNotes.Client.Windows.Implementations.Tasks;
using Xemio.SmartNotes.Client.Windows.Views.CreateFolder;
using Xemio.SmartNotes.Client.Windows.Views.EditFolder;
using Xemio.SmartNotes.Shared.Entities.Notes;

namespace Xemio.SmartNotes.Client.Windows.Views.Shell.AllNotes
{
    public class AllNotesViewModel : Screen, IHandle<FolderCreatedEvent>, IHandleWithTask<SelectedFolderEvent>, IHandle<FolderDeletedEvent>, IHandle<FolderEditedEvent>
    {
        #region Fields
        private readonly WebServiceClient _client;
        private readonly DisplayManager _displayManager;
        private readonly ITaskExecutor _taskExecutor;
        private readonly IEventAggregator _eventAggregator;

        private BindableCollection<FolderViewModel> _folders;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        public ILogger Logger { get; set; }
        /// <summary>
        /// Gets or sets the folders.
        /// </summary>
        public BindableCollection<FolderViewModel> Folders
        {
            get { return this._folders; }
            set
            {
                if (this._folders != value)
                {
                    this._folders = value;
                    this.NotifyOfPropertyChange(() => this.Folders);
                }
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="AllNotesViewModel"/> class.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="displayManager">The display manager.</param>
        /// <param name="taskExecutor">The task executor.</param>
        /// <param name="eventAggregator">The selectedFolderEvent aggregator.</param>
        public AllNotesViewModel(WebServiceClient client, DisplayManager displayManager, ITaskExecutor taskExecutor, IEventAggregator eventAggregator)
        {
            this.Logger = NullLogger.Instance;

            this._client = client;
            this._displayManager = displayManager;
            this._taskExecutor = taskExecutor;
            this._eventAggregator = eventAggregator;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Creates a new folder.
        /// </summary>
        public void CreateFolder()
        {
            FolderViewModel selectedFolder = this.GetAllFolders().SingleOrDefault(f => f.IsSelected);

            var createFolderViewModel = IoC.Get<CreateFolderViewModel>();
            createFolderViewModel.ParentFolderId = selectedFolder != null ? selectedFolder.FolderId : null;

            dynamic settings = new ExpandoObject();
            settings.ResizeMode = ResizeMode.CanMinimize;

            this._displayManager.Windows.ShowDialog(createFolderViewModel, null, settings);
        }
        /// <summary>
        /// Edits the currently selected folder.
        /// </summary>
        public void EditFolder()
        {
            FolderViewModel selectedFolder = this.GetAllFolders().SingleOrDefault(f => f.IsSelected);
            if (selectedFolder == null)
                return;

            FolderViewModel parentFolder = this.GetAllFolders().SingleOrDefault(f => f.SubFolders.Contains(selectedFolder));

            var editFolderViewModel = IoC.Get<EditFolderViewModel>();
            editFolderViewModel.UserId = this._client.Session.User.Id;
            editFolderViewModel.FolderId = selectedFolder.FolderId;
            editFolderViewModel.ParentFolderId = parentFolder != null ? parentFolder.FolderId : null;
            editFolderViewModel.FolderTags = string.Join(", ", selectedFolder.Tags);
            editFolderViewModel.FolderName = selectedFolder.Name;

            dynamic settings = new ExpandoObject();
            settings.ResizeMode = ResizeMode.CanMinimize;

            this._displayManager.Windows.ShowDialog(editFolderViewModel, null, settings);
        }

        /// <summary>
        /// Deletes the currently selected folder.
        /// </summary>
        public void DeleteFolder()
        {
            FolderViewModel selectedFolder = this.GetAllFolders().SingleOrDefault(f => f.IsSelected);
            if (selectedFolder == null)
                return;

            var task = IoC.Get<DeleteFolderTask>();
            task.FolderId = selectedFolder.FolderId;
            task.FolderName = selectedFolder.Name;

            this._taskExecutor.StartTask(task);
        }
        #endregion

        #region Overrides of Screen
        /// <summary>
        /// Called when initializing.
        /// </summary>
        protected override async void OnInitialize()
        {
            this._eventAggregator.Subscribe(this);
            await this.LoadFolders();
        }
        #endregion

        #region Implementation of IHandle<FolderCreatedEvent>
        /// <summary>
        /// Handles the <see cref="FolderCreatedEvent"/>.
        /// </summary>
        /// <param name="message">The FolderCreatedEvent.</param>
        public void Handle(FolderCreatedEvent message)
        {
            var folderViewModel = IoC.Get<FolderViewModel>();
            folderViewModel.Initialize(message.Folder, false);

            if (string.IsNullOrWhiteSpace(message.Folder.ParentFolderId))
            {
                //It is a new root folder
                this.Folders.Add(folderViewModel);
            }
            else
            {
                //We search for its parent folder and add it to its SubFolders
                FolderViewModel parentFolder = this.GetAllFolders().FirstOrDefault(f => f.FolderId == message.Folder.ParentFolderId);
                if (parentFolder != null)
                {
                    parentFolder.SubFolders.Add(folderViewModel);
                }
            }
        }
        #endregion
        
        #region Implementation of IHandle<FolderEditedEvent>
        /// <summary>
        /// Handles the <see cref="FolderEditedEvent"/>.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Handle(FolderEditedEvent message)
        {
            FolderViewModel editedFolder = this.GetAllFolders().SingleOrDefault(f => f.FolderId == message.Folder.Id);

            if (editedFolder != null)
                editedFolder.Initialize(message.Folder, false);
        }
        #endregion

        #region Implementation of IHandle<FolderDeletedEvent>
        /// <summary>
        /// Handles the <see cref="FolderDeletedEvent"/>.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Handle(FolderDeletedEvent message)
        {
            FolderViewModel deletedFolder = this.GetAllFolders().SingleOrDefault(f => f.FolderId == message.FolderId);

            if (deletedFolder == null)
                return;

            FolderViewModel parentFolder = this.GetAllFolders().SingleOrDefault(f => f.SubFolders.Contains(deletedFolder));
            if (parentFolder != null)
            {
                parentFolder.SubFolders.Remove(deletedFolder);
            }
            else
            {
                this.Folders.Remove(deletedFolder);
            }
        }
        #endregion

        #region Implementation of IHandleWithTask<SelectedFolderEvent>
        /// <summary>
        /// Handles the <see cref="SelectedFolderEvent"/>.
        /// </summary>
        /// <param name="selectedFolderEvent">The SelectedFolderEvent.</param>
        public async Task Handle(SelectedFolderEvent selectedFolderEvent)
        {
            HttpResponseMessage response = await this._client.Notes.GetAllNotes(selectedFolderEvent.FolderId);
            if (response.StatusCode == HttpStatusCode.Found)
            {
                //TODO: Load notes and display them in the grid
            }
            else
            {
                string message = await response.Content.ReadAsStringAsync();
                this._displayManager.Messages.ShowMessageBox(message, ClientMessages.Error, MessageBoxButton.OK, MessageBoxImage.Error);

                this.Logger.ErrorFormat("Error while loading notes from folder '{0}': {1}", selectedFolderEvent.FolderId, message);
            }
        }
        #endregion
        
        #region Private Methods
        /// <summary>
        /// Loads the folders.
        /// </summary>
        private async Task LoadFolders()
        {
            HttpResponseMessage response = await this._client.Folders.GetAllFolders(null);
            if (response.StatusCode == HttpStatusCode.Found)
            {
                Folder[] folders = await response.Content.ReadAsAsync<Folder[]>();

                this.Folders = new BindableCollection<FolderViewModel>(folders.Select(f =>
                {
                    var viewModel = IoC.Get<FolderViewModel>();
                    viewModel.Initialize(f);

                    return viewModel;
                }));
            }
            else
            {
                string message = await response.Content.ReadAsStringAsync();
                this.Logger.ErrorFormat("Error while loading all folders: {0}", message);

                this._displayManager.Messages.ShowMessageBox(message, ClientMessages.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// Gets all folders.
        /// </summary>
        /// <param name="folders">The folders.</param>
        private IEnumerable<FolderViewModel> GetAllFolders(IEnumerable<FolderViewModel> folders = null)
        {
            if (folders == null)
                folders = this.Folders;

            var allSubFolders = new List<FolderViewModel>();

            foreach (FolderViewModel folder in folders)
            { 
                allSubFolders.Add(folder);
                allSubFolders.AddRange(this.GetAllFolders(folder.SubFolders));
            }

            return allSubFolders;
        }
        #endregion
    }
}
