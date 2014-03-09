using System;
using System.Collections.Generic;
using Caliburn.Micro;
using Xemio.SmartNotes.Shared.Entities.Notes;

namespace Xemio.SmartNotes.Client.Windows.ViewParts
{
    public class NoteViewModel : PropertyChangedBase
    {
        #region Fields
        private string _noteId;
        private string _title;
        private string _content;
        private ICollection<string> _tags;
        private string _folderId;
        private DateTimeOffset _createdDate;
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

            this.Note = note;

            this.NoteId = note.Id;
            this.Title = note.Name;
            this.Content = note.Content;
            this.Tags = note.Tags;
            this.FolderId = note.FolderId;
            this.CreatedDate = note.CreatedDate;
        }
        #endregion
    }
}
