using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xemio.SmartNotes.Shared.Entities.Notes;

namespace Xemio.SmartNotes.Client.Windows.Data.Events
{
    /// <summary>
    /// Fired when a note was edited.
    /// </summary>
    public class NoteEditedEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NoteEditedEvent"/> class.
        /// </summary>
        /// <param name="note">The note.</param>
        public NoteEditedEvent(Note note)
        {
            this.Note = note;
        }

        /// <summary>
        /// Gets the note.
        /// </summary>
        public Note Note { get; private set; }
    }
}
