using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Xemio.SmartNotes.Client.Abstractions.Tasks;
using Xemio.SmartNotes.Client.Windows.Implementations.Tasks;

namespace Xemio.SmartNotes.Client.Windows.Views.CreateFolder
{
    public class CreateFolderViewModel : Screen
    {
        #region Fields
        private readonly ITaskExecutor _taskExecutor;

        private string _folderName;
        private BindableCollection<string> _tags;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateFolderViewModel"/> class.
        /// </summary>
        /// <param name="taskExecutor">The task executor.</param>
        public CreateFolderViewModel(ITaskExecutor taskExecutor)
        {
            this._taskExecutor = taskExecutor;

            this.Tags = new BindableCollection<string> {"hallo", "test", "123"};

            this.DisplayName = "Xemio Notes";
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the parent folder identifier.
        /// </summary>
        public string ParentFolderId { get; set; }
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
                    this.NotifyOfPropertyChange(() => this.CanCreateFolder);
                }
            }
        }
        /// <summary>
        /// Gets or sets the tags of the folder.
        /// </summary>
        public BindableCollection<string> Tags
        {
            get { return this._tags; }
            set
            {
                if (this._tags != value)
                {
                    this._tags = value;
                    this.NotifyOfPropertyChange(() => this.Tags);
                }
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Gets a value indicating whether you can create the folder.
        /// </summary>
        public bool CanCreateFolder
        {
            get { return string.IsNullOrWhiteSpace(this.FolderName) == false; }
        }
        /// <summary>
        /// Creates the folder.
        /// </summary>
        public void CreateFolder()
        {
            var task = IoC.Get<CreateFolderTask>();
            task.FolderName = this.FolderName;
            task.FolderTags = this.Tags.ToArray();
            task.ParentFolderId = this.ParentFolderId;

            this._taskExecutor.StartTask(task);

            this.TryClose();
        }
        #endregion
    }
}
