using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Castle.Core.Logging;
using Xemio.SmartNotes.Client.Shared.WebService;
using Xemio.SmartNotes.Client.Windows.Data.Events;
using Xemio.SmartNotes.Models.Entities.Notes;

namespace Xemio.SmartNotes.Client.Windows.Views.Shell.AllNotes
{
    public class FolderViewModel : PropertyChangedBase
    {
        #region Fields
        private readonly WebServiceClient _client;
        private readonly IEventAggregator _eventAggregator;

        private string _name;
        private bool _isExpanded;
        private bool _isSelected;

        private bool _alreadyLoadedSubFolders;
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FolderViewModel"/> class.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="eventAggregator">The event aggregator.</param>
        public FolderViewModel(WebServiceClient client, IEventAggregator eventAggregator)
        {
            this.Logger = NullLogger.Instance;
            this.SubFolders = new BindableCollection<FolderViewModel>();
            this.Tags = new BindableCollection<string>();

            this._client = client;
            this._eventAggregator = eventAggregator;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        public ILogger Logger { get; set; }
        /// <summary>
        /// Gets or sets the folder identifier.
        /// </summary>
        public string FolderId { get; set; }
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name
        {
            get { return this._name; }
            set
            {
                if (value != _name)
                { 
                    this._name = value;
                    this.NotifyOfPropertyChange(() => this.Name);
                }
            }
        }
        /// <summary>
        /// Gets or sets the tags.
        /// </summary>
        public BindableCollection<string> Tags { get; set; }
        /// <summary>
        /// Gets or sets the sub folders.
        /// </summary>
        public BindableCollection<FolderViewModel> SubFolders { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this folder is expanded.
        /// </summary>
        public bool IsExpanded
        {
            get { return this._isExpanded; }
            set
            {
                if (this._isExpanded != value)
                { 
                    this._isExpanded = value;
                    this.NotifyOfPropertyChange(() => this.IsExpanded);
                    
                    foreach (FolderViewModel subFolder in this.SubFolders)
                    {
                        subFolder.LoadSubFolders();
                    }
                }
            }
        }
        /// <summary>
        /// Gets or sets a value indicating whether this folder is selected..
        /// </summary>
        public bool IsSelected
        {
            get { return this._isSelected; }
            set
            {
                if (this._isSelected != value)
                {
                    this._isSelected = value;
                    this.NotifyOfPropertyChange(() => this.IsSelected);

                    if (this._isSelected)
                        //We need this kinda dumb approach because in a TreeView you don't have a bindable SelectedItem property
                        //We try to get around by this, by binding the IsSelected property of every node and fire an event when the folder changes
                        this._eventAggregator.Publish(new SelectedFolderEvent(this.FolderId));
                }
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Initializes the specified <paramref name="folder"/>.
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <param name="loadSubFolders">if set to <c>true</c> we load the sub folders.</param>
        public void Initialize(Folder folder, bool loadSubFolders = true)
        {
            this.FolderId = folder.Id;
            this.Name = folder.Name;
            this.Tags = new BindableCollection<string>(folder.Tags);

            if (loadSubFolders)
                this.LoadSubFolders();
        }
        /// <summary>
        /// Loads the sub folders.
        /// </summary>
        private async void LoadSubFolders()
        {
            if (this._alreadyLoadedSubFolders)
                return;
            
            this._alreadyLoadedSubFolders = true;

            HttpResponseMessage response = await this._client.Folders.GetAllFolders(this.FolderId);
            if (response.StatusCode == HttpStatusCode.Found)
            {
                Folder[] folders = await response.Content.ReadAsAsync<Folder[]>();

                this.SubFolders.AddRange(folders.Select(f =>
                {
                    var viewModel = IoC.Get<FolderViewModel>();
                    //We need to use "Wait" here because we can't use async-await in LINQ lambdas
                    viewModel.Initialize(f, false);

                    return viewModel;
                }));
            }
            else
            {
                string message = await response.Content.ReadAsStringAsync();
                this.Logger.ErrorFormat("Error while loading subfolders from folder '{0}': {1}", this.FolderId, message);
            }
        }
        #endregion
    }
}
