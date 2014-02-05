using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xemio.SmartNotes.Shared.Entities.Notes;

namespace Xemio.SmartNotes.Client.Windows.Data.Events
{
    /// <summary>
    /// Fired when a folder was edited.
    /// </summary>
    public class FolderEditedEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FolderEditedEvent"/> class.
        /// </summary>
        /// <param name="folder">The folder.</param>
        public FolderEditedEvent(Folder folder)
        {
            Folder = folder;
        }

        /// <summary>
        /// Gets the folder that was edited.
        /// </summary>
        public Folder Folder { get; private set; }
    }
}
