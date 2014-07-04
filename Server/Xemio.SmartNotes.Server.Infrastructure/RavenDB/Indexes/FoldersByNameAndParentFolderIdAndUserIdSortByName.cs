using System.Linq;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;
using Xemio.SmartNotes.Shared.Entities.Notes;

namespace Xemio.SmartNotes.Server.Infrastructure.RavenDB.Indexes
{
    public class FoldersByNameAndParentFolderIdAndUserIdSortByName : AbstractIndexCreationTask<Folder>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FoldersByNameAndParentFolderIdAndUserIdSortByName"/> class.
        /// </summary>
        public FoldersByNameAndParentFolderIdAndUserIdSortByName()
        {
            this.Map = folders => from folder in folders
                                  select new
                                  {
                                      folder.Name,
                                      folder.ParentFolderId,
                                      folder.UserId
                                  };

            this.Sort(f => f.Name, SortOptions.String);
        }

        /// <summary>
        /// Gets the name of the index.
        /// </summary>
        public override string IndexName
        {
            get { return "Folders/ByNameAndParentFolderIdAndUserIdSortByName"; }
        }
    }
}