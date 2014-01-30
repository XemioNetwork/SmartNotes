using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Xemio.SmartNotes.Client.Abstractions.Tasks;
using Xemio.SmartNotes.Client.Windows.Extensions;
using Xemio.SmartNotes.Client.Windows.Implementations.Tasks;

namespace Xemio.SmartNotes.Client.Windows.Views.CreateFolder
{
    public class CreateFolderViewModel : Screen
    {
        #region Fields
        private readonly ITaskExecutor _taskExecutor;

        private string _parentFolderId;
        private string _folderName;
        private string _folderTags;
        private bool _isRootFolder;

        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateFolderViewModel"/> class.
        /// </summary>
        /// <param name="taskExecutor">The task executor.</param>
        public CreateFolderViewModel(ITaskExecutor taskExecutor)
        {
            this._taskExecutor = taskExecutor;

            this.DisplayName = "Xemio Notes";
        }
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the parent folder identifier.
        /// </summary>
        public string ParentFolderId
        {
            get { return this._parentFolderId; }
            set
            {
                if (this._parentFolderId != value)
                { 
                    this._parentFolderId = value;

                    this.IsRootFolder = string.IsNullOrWhiteSpace(this.ParentFolderId);

                    this.NotifyOfPropertyChange(() => this.ParentFolderId);
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
                    this.NotifyOfPropertyChange(() => this.CanCreateFolder);
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
        /// Gets a value indicating whether the new folder will be a sub folder.
        /// </summary>
        public bool IsRootFolder
        {
            get { return _isRootFolder; }
            set
            {
                if (this._isRootFolder != value)
                { 
                    this._isRootFolder = value;
                    this.NotifyOfPropertyChange(() => this.IsRootFolder);
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
            task.FolderTags = this.FolderTags.GetTags();
            task.ParentFolderId = this.IsRootFolder ? null :  this.ParentFolderId;

            this._taskExecutor.StartTask(task);

            this.TryClose();
        }
        #endregion
    }
}
