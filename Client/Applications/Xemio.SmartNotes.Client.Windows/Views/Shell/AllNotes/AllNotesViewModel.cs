using System;
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
using Xemio.SmartNotes.Client.Windows.ViewParts;
using Xemio.SmartNotes.Client.Windows.Views.CreateFolder;
using Xemio.SmartNotes.Client.Windows.Views.EditFolder;
using Xemio.SmartNotes.Shared.Entities.Notes;
using Xemio.SmartNotes.Shared.Extensions;

namespace Xemio.SmartNotes.Client.Windows.Views.Shell.AllNotes
{
    public class AllNotesViewModel : Screen, 
        IHandle<FolderCreatedEvent>, 
        IHandleWithTask<SelectedFolderEvent>, 
        IHandle<FolderDeletedEvent>, 
        IHandle<FolderEditedEvent>,
        IHandle<FolderMovedEvent>,
        IHandle<NoteMovedEvent>

    {
        #region Fields
        private readonly WebServiceClient _client;
        private readonly DisplayManager _displayManager;
        private readonly ITaskExecutor _taskExecutor;
        private readonly IEventAggregator _eventAggregator;

        private BindableCollection<FolderViewModel> _folders;
        private BindableCollection<NoteViewModel> _notes;
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
        /// <summary>
        /// Gets or sets the notes.
        /// </summary>
        public BindableCollection<NoteViewModel> Notes
        {
            get { return this._notes; }
            set
            {
                if (this._notes != value)
                { 
                    this._notes = value;
                    this.NotifyOfPropertyChange(() => this.Notes);
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

            var editFolderViewModel = IoC.Get<EditFolderViewModel>();
            editFolderViewModel.Folder = selectedFolder.Folder;

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
        /// <summary>
        /// Moves the specified <paramref name="noteToMove"/> to the specified <paramref name="newParentFolder"/>.
        /// </summary>
        /// <param name="noteToMove">The note to move.</param>
        /// <param name="newParentFolder">The new parent folder.</param>
        public void MoveNote(NoteViewModel noteToMove, FolderViewModel newParentFolder)
        {
            if (noteToMove == null)
                throw new ArgumentNullException("noteToMove");

            if (newParentFolder == null)
                throw new ArgumentNullException("newParentFolder");

            //We want to move the note to it's current parent folder
            if (noteToMove.FolderId == newParentFolder.FolderId)
                return;

            var task = IoC.Get<MoveNoteTask>();
            task.Note = noteToMove.Note.DeepClone();
            task.Note.FolderId = newParentFolder.FolderId;

            this._taskExecutor.StartTask(task);
        }
        /// <summary>
        /// Moves the specified <paramref name="folderToMove"/> to the specified <paramref name="newParentFolder"/>.
        /// </summary>
        /// <param name="folderToMove">The folder to move.</param>
        /// <param name="newParentFolder">The new parent.</param>
        public void MoveFolder(FolderViewModel folderToMove, FolderViewModel newParentFolder)
        {
            if (folderToMove == null)
                throw new ArgumentNullException("folderToMove");
            
            //We want to move the folder to itself
            if (folderToMove == newParentFolder)
                return;

            //We want to move the folder to a subfolder of itself
            if (newParentFolder != null && folderToMove.SubFolders.Contains(newParentFolder))
                return;

            string newParentFolderId = newParentFolder != null ? newParentFolder.Folder.Id : null;

            //The parent folder did not change
            if (folderToMove.Folder.ParentFolderId == newParentFolderId)
                return;
            
            var task = IoC.Get<MoveFolderTask>();
            task.Folder = folderToMove.Folder.DeepClone();
            task.Folder.ParentFolderId = newParentFolderId;

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
            FolderViewModel editedFolder = this.GetAllFolders().Single(f => f.FolderId == message.Folder.Id);

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
                Note[] notes = await response.Content.ReadAsAsync<Note[]>();

                this.Notes = new BindableCollection<NoteViewModel>(notes.Select(f =>
                {
                    var viewModel = IoC.Get<NoteViewModel>();
                    viewModel.Initialize(f);

                    return viewModel;
                }));
            }
            else
            {
                string message = await response.Content.ReadAsStringAsync();
                this._displayManager.Messages.ShowMessageBox(message, ClientMessages.Error, MessageBoxButton.OK, MessageBoxImage.Error);

                this.Logger.ErrorFormat("Error while loading notes from folder '{0}': {1}", selectedFolderEvent.FolderId, message);
            }
        }
        #endregion

        #region Implementation of IHandle<FolderMovedEvent>
        /// <summary>
        /// Handles the <see cref="FolderMovedEvent"/>.
        /// </summary>
        /// <param name="message">The FolderMovedEvent.</param>
        public void Handle(FolderMovedEvent message)
        {
            FolderViewModel movedFolder = this.GetAllFolders().Single(f => f.FolderId == message.Folder.Id);
            movedFolder.Initialize(message.Folder);

            //Remove the folder from it's old parent folder
            FolderViewModel oldParentFolder = this.GetAllFolders().SingleOrDefault(f => f.SubFolders.Contains(movedFolder));
            if (oldParentFolder == null)
            {
                this.Folders.Remove(movedFolder);
            }
            else
            {
                oldParentFolder.SubFolders.Remove(movedFolder);
            }
            
            //Add the folder to it's new parent folder
            FolderViewModel newParentFolder = this.GetAllFolders().SingleOrDefault(f => f.FolderId == message.Folder.ParentFolderId);
            if (newParentFolder == null)
            {
                this.Folders.Add(movedFolder);
            }
            else
            {
                newParentFolder.SubFolders.Add(movedFolder);
            }
        }
        #endregion

        #region Implementation of IHandle<NoteMovedEvent>
        /// <summary>
        /// Handles the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Handle(NoteMovedEvent message)
        {
            NoteViewModel movedNote = this.Notes.FirstOrDefault(f => f.NoteId == message.Note.Id);

            //The note moved to another folder
            if (movedNote != null && movedNote.FolderId != message.Note.FolderId)
            {
                this.Notes.Remove(movedNote);
                return;
            }

            //The note moved to the current folder
            FolderViewModel selectedFolder = this.GetAllFolders().FirstOrDefault(f => f.IsSelected);
            if (selectedFolder != null && message.Note.FolderId == selectedFolder.FolderId)
            {
                var viewModel = IoC.Get<NoteViewModel>();
                viewModel.Initialize(message.Note);

                this.Notes.Add(viewModel);
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

                FolderViewModel firstFolder = this.Folders.FirstOrDefault();
                if (firstFolder != null)
                { 
                    firstFolder.IsSelected = true;
                    await this.Handle(new SelectedFolderEvent(firstFolder.FolderId));
                }
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
