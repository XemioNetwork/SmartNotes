using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raven.Client.Indexes;
using Xemio.SmartNotes.Entities.Notes;

namespace Xemio.SmartNotes.Infrastructure.Raven.Indexes
{
    /// <summary>
    /// Enables querying for <see cref="Folder"/>s on their <see cref="Folder.UserId"/> property.
    /// </summary>
    public class FoldersByUserId : AbstractIndexCreationTask<Folder>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FoldersByUserId"/> class.
        /// </summary>
        public FoldersByUserId()
        {
            this.Map = folders => from folder in folders
                                  select new {folder.UserId};
        }

        /// <summary>
        /// Gets the name of the index.
        /// </summary>
        public override string IndexName
        {
            get { return "Folders/ByUserId"; }
        }
    }
}
