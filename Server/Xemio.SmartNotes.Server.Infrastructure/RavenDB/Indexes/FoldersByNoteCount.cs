using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;
using Xemio.SmartNotes.Shared.Entities.Notes;
using Xemio.SmartNotes.Shared.Models;

namespace Xemio.SmartNotes.Server.Infrastructure.RavenDB.Indexes
{
    public class FoldersByNoteCount : AbstractIndexCreationTask<Note, FolderNoteCount>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FoldersByNoteCount"/> class.
        /// </summary>
        public FoldersByNoteCount()
        {
            this.Map = notes => from note in notes
                                select new
                                {
                                    note.FolderId,
                                    NoteCount = 1
                                };

            this.Reduce = results => from result in results
                                     group result by result.FolderId
                                     into g
                                     select new
                                     {
                                         FolderId = g.Key,
                                         NoteCount = g.Sum(f => f.NoteCount)
                                     };
            
            this.Sort(f => f.NoteCount, SortOptions.Int);
        }
        /// <summary>
        /// Gets the name of the index.
        /// </summary>
        public override string IndexName
        {
            get { return "Folders/ByNoteCount"; }
        }
    }
}
