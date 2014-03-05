using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Xemio.SmartNotes.Shared.Entities.Notes;

namespace Xemio.SmartNotes.Client.Windows.Views.Shell.AllNotes
{
    public class NoteViewModel : PropertyChangedBase
    {
        #region Fields
        private string _title;
        private string _content;
        private ICollection<string> _tags;
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
        }
        #endregion
    }
}
