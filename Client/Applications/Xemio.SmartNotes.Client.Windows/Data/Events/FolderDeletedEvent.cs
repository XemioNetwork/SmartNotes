using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xemio.SmartNotes.Client.Windows.Data.Events
{
    public class FolderDeletedEvent
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="FolderDeletedEvent"/> class.
        /// </summary>
        /// <param name="folderId">The folder identifier.</param>
        public FolderDeletedEvent(string folderId)
        {
            this.FolderId = folderId;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the folder identifier.
        /// </summary>
        public string FolderId { get; private set; }
        #endregion
    }
}
