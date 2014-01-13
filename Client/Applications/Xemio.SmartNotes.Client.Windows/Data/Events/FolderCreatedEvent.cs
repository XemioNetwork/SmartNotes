using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xemio.SmartNotes.Models.Entities.Notes;

namespace Xemio.SmartNotes.Client.Windows.Data.Events
{
    /// <summary>
    /// Fired when a folder was created.
    /// </summary>
    public class FolderCreatedEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FolderCreatedEvent"/> class.
        /// </summary>
        /// <param name="folder">The folder.</param>
        public FolderCreatedEvent(Folder folder)
        {
            this.Folder = folder;
        }

        /// <summary>
        /// Gets the folder.
        /// </summary>
        public Folder Folder { get; private set; }
    }
}
