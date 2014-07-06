using System;
using System.Collections.Generic;
using Caliburn.Micro;
using Xemio.SmartNotes.Client.Shared.Tasks;
using Xemio.SmartNotes.Client.Windows.Data.Events;
using Xemio.SmartNotes.Client.Windows.Implementations.Tasks;
using Xemio.SmartNotes.Shared.Entities.Notes;

namespace Xemio.SmartNotes.Client.Windows.ViewParts
{
    public class NoteViewModel : PropertyChangedBase, IHandle<NoteEditedEvent>
    {
        #region Fields
        private readonly ITaskExecutor _taskExecutor;

        private bool _isInitialized;
        private bool _isUpdatingFavorite;

        private string _noteId;
        private string _title;
        private string _content;
        private ICollection<string> _tags;
        private string _folderId;
        private DateTimeOffset _createdDate;
        private bool _isFavorite;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the note.
        /// </summary>
        public Note Note { get; private set; }
        /// <summary>
        /// Gets the note identifier.
        /// </summary>
        public string NoteId
        {
            get { return this._noteId; }
            private set
            {
                if (this._noteId != value)
                { 
                    this._noteId = value;
                    this.NotifyOfPropertyChange(() => this.NoteId);
                }
            }
        }
        /// <summary>
        /// Gets the title.
        /// </summary>
        public string Title
        {
            get { return this._title; }
            private set
            {
                if (this._title != value)
                { 
                    this._title = value;
                    this.NotifyOfPropertyChange(() => this.Title);
                }
            }
        }
        /// <summary>
        /// Gets the content.
        /// </summary>
        public string Content
        {
            get { return this._content; }
            private set
            {
                if (this._content != value)
                { 
                    this._content = value;
                    this.NotifyOfPropertyChange(() => this.Content);
                }
            }
        }
        /// <summary>
        /// Gets the tags.
        /// </summary>
        public ICollection<string> Tags
        {
            get { return _tags; }
            private set
            {
                if (this._tags != value)
                { 
                    this._tags = value;
                    this.NotifyOfPropertyChange(() => this.Tags);
                }
            }
        }
        /// <summary>
        /// Gets the folder identifier.
        /// </summary>
        public string FolderId
        {
            get { return this._folderId; }
            private set
            {
                if (this._folderId != value)
                {
                    this._folderId = value;
                    this.NotifyOfPropertyChange(() => this.FolderId);
                }
            }
        }
        /// <summary>
        /// Gets the created date.
        /// </summary>
        public DateTimeOffset CreatedDate
        {
            get { return this._createdDate; }
            private set
            {
                if (this._createdDate != value)
                {
                    this._createdDate = value;
                    this.NotifyOfPropertyChange(() => this.CreatedDate);
                }
            }
        }
        /// <summary>
        /// Gets or sets a value indicating whether this note is marked as a favorite.
        /// </summary>
        public bool IsFavorite
        {
            get { return this._isFavorite; }
            set
            {
                if (this._isFavorite != value)
                {
                    if (this._isInitialized == false || this._isUpdatingFavorite)
                        return;

                    this._isUpdatingFavorite = true;

                    if (this.IsFavorite)
                    {
                        var task = IoC.Get<UnmarkNoteAsFavoriteTask>();
                        task.NoteId = this.NoteId;

                        this._taskExecutor.StartTask(task);
                    }
                    else
                    {
                        var task = IoC.Get<MarkNoteAsFavoriteTask>();
                        task.NoteId = this.NoteId;

                        this._taskExecutor.StartTask(task);
                    }
                }
            }
        }
        #endregion 

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="NoteViewModel"/> class.
        /// </summary>
        /// <param name="eventAggregator">The event aggregator.</param>
        /// <param name="taskExecutor">The task executor.</param>
        public NoteViewModel(IEventAggregator eventAggregator, ITaskExecutor taskExecutor)
        {
            this._taskExecutor = taskExecutor;

            eventAggregator.Subscribe(this);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Initializes this <see cref="NoteViewModel"/> with the specified <paramref name="note"/>.
        /// </summary>
        /// <param name="note">The note.</param>
        public void Initialize(Note note)
        {
            if (note == null)
                throw new ArgumentNullException("note");

            this._isInitialized = false;

            this.Note = note;

            this.NoteId = note.Id;
            this.Title = note.Name;
            this.Content = note.Content;
            this.Tags = note.Tags;
            this.FolderId = note.FolderId;
            this.CreatedDate = note.CreatedDate;
            this._isFavorite = note.IsFavorite;
            this.NotifyOfPropertyChange(() => this.IsFavorite);

            this._isInitialized = true;
        }
        #endregion

        #region Implementation of IHandle<NoteEditedEvent>
        /// <summary>
        /// Handles the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Handle(NoteEditedEvent message)
        {
            if (message.Note.Id == this.NoteId)
            {
                this.Initialize(message.Note);

                this._isUpdatingFavorite = false;
            }
        }
        #endregion
    }
}
