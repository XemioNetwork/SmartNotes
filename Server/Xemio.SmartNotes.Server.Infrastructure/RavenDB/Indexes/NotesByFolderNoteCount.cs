using System.Linq;
using Raven.Client.Indexes;
using Xemio.SmartNotes.Shared.Entities.Notes;

namespace Xemio.SmartNotes.Server.Infrastructure.RavenDB.Indexes
{
    public class NotesByFolderNoteCount : AbstractIndexCreationTask<Note, NotesByFolderNoteCount.Result>
    {
        /// <summary>
        /// The result of the <see cref="NotesByFolderNoteCount"/> index.
        /// </summary>
        public class Result
        {
            public string FolderId { get; set; }
            public int Count { get; set; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotesByFolderNoteCount"/> class.
        /// </summary>
        public NotesByFolderNoteCount()
        {
            this.Map = notes => from note in notes
                                select new
                                {
                                    note.FolderId,
                                    Count = 1
                                };

            this.Reduce = results => from result in results
                                     group result by result.FolderId
                                     into g
                                     select new Result
                                     {
                                         FolderId = g.Key,
                                         Count = g.Sum(f => f.Count)
                                     };
        }
        /// <summary>
        /// Gets the name of the index.
        /// </summary>
        public override string IndexName
        {
            get { return "Notes/ByFolderNoteCount"; }
        }
    }
}