using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xemio.SmartNotes.Shared.Entities.Notes;

namespace Xemio.SmartNotes.Client.Windows.Data.Events
{
    /// <summary>
    /// Fired when the "IsFavorite" property of a note has changed.
    /// </summary>
    public class NoteIsFavoriteChangedEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NoteIsFavoriteChangedEvent"/> class.
        /// </summary>
        /// <param name="note">The note.</param>
        public NoteIsFavoriteChangedEvent(Note note)
        {
            this.Note = note;
        }

        /// <summary>
        /// Gets the note.
        /// </summary>
        public Note Note { get; private set; }
    }
}
