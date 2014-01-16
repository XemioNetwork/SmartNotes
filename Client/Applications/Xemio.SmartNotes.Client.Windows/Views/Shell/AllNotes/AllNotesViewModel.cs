using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Caliburn.Micro;
using Castle.Core.Logging;
using Xemio.CommonLibrary.Security;
using Xemio.SmartNotes.Client.Abstractions.Tasks;
using Xemio.SmartNotes.Client.Shared.WebService;
using Xemio.SmartNotes.Client.Windows.Data.Events;
using Xemio.SmartNotes.Client.Windows.Implementations.Tasks;
using Xemio.SmartNotes.Models.Entities.Notes;

namespace Xemio.SmartNotes.Client.Windows.Views.Shell.AllNotes
{
    public class AllNotesViewModel : Screen, IHandle<FolderCreatedEvent>, IHandleWithTask<SelectedFolderEvent>
    {
        #region Fields
        private readonly WebServiceClient _client;
        private readonly ITaskExecutor _taskExecutor;

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
        /// <param name="taskExecutor">The task executor.</param>
        /// <param name="eventAggregator">The selectedFolderEvent aggregator.</param>
        public AllNotesViewModel(WebServiceClient client, ITaskExecutor taskExecutor, IEventAggregator eventAggregator)
        {
            this.Logger = NullLogger.Instance;

            this._client = client;
            this._taskExecutor = taskExecutor;
            
            eventAggregator.Subscribe(this);
        }
        #endregion

        #region Overrides of Screen
        /// <summary>
        /// Called when initializing.
        /// </summary>
        protected override async void OnInitialize()
        {
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

        #region Implementation of IHandle<SelectedFolderEvent>
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
                this.Logger.ErrorFormat("Error while loading notes from folder '{0}': {1}", selectedFolderEvent.FolderId, message);

                //TODO: Display error to user
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

                //TODO: Display error to user
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
