using System;
using System.Collections.Generic;
using Caliburn.Micro;
using Xemio.SmartNotes.Shared.Entities.Notes;

namespace Xemio.SmartNotes.Client.Windows.ViewParts
{
    public class NoteViewModel : PropertyChangedBase
    {
        #region Fields
        private string _title;
        private string _content;
        private ICollection<string> _tags;
        private DateTimeOffset _createdDate;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the note.
        /// </summary>
        public Note Note { get; private set; }
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

            this.Title = note.Name;
            this.Content = note.Content;
            this.Tags = note.Tags;
            this.CreatedDate = note.CreatedDate;
        }
        #endregion
    }
}
