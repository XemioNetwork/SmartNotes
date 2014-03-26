using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Xemio.SmartNotes.Shared.Entities.Notes;

namespace Xemio.SmartNotes.Client.Windows.Views.WatchNote
{
    public class WatchNoteViewModel : Screen
    {
        #region Fields
        private string _noteContent;
        private string _noteName;
        private BindableCollection<string> _noteTags;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the note title.
        /// </summary>
        public string NoteName
        {
            get { return this._noteName; }
            private set
            {
                if (this._noteName != value)
                {
                    this._noteName = value;
                    this.NotifyOfPropertyChange(() => this.NoteName);
                }
            }
        }
        /// <summary>
        /// Gets or sets the note text.
        /// </summary>
        public string NoteContent
        {
            get { return this._noteContent; }
            private set
            {
                if (this._noteContent != value)
                { 
                    this._noteContent = value;
                    this.NotifyOfPropertyChange(() => this.NoteContent);
                }
            }
        }
        /// <summary>
        /// Gets or sets the note tags.
        /// </summary>
        public BindableCollection<string> NoteTags
        {
            get { return this._noteTags; }
            private set
            {
                if (this._noteTags != value)
                {
                    this._noteTags = value;
                    this.NotifyOfPropertyChange(() => this.NoteTags);
                }
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="WatchNoteViewModel"/> class.
        /// </summary>
        public WatchNoteViewModel()
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Initializes this viewmodel with the specified <paramref name="note"/>.
        /// </summary>
        /// <param name="note">The note.</param>
        public void Initialize(Note note)
        {
            if (note == null)
                throw new ArgumentNullException("note");

            this.NoteName = note.Name;
            this.NoteContent = note.Content;
            this.NoteTags = new BindableCollection<string>(note.Tags);
        }
        #endregion
    }
}
