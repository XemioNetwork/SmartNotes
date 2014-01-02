using System.Linq;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;
using Xemio.SmartNotes.Models.Entities.Notes;

namespace Xemio.SmartNotes.Server.Infrastructure.Raven.Indexes
{
    /// <summary>
    /// Enables fulltext search for <see cref="Note"/>s.
    /// </summary>
    public class NotesBySearchTextAndFolderId : AbstractIndexCreationTask<Note, NotesBySearchTextAndFolderId.Result>
    {
        /// <summary>
        /// The result of the <see cref="NotesBySearchTextAndFolderId"/> index.
        /// </summary>
        public class Result
        {
            public string[] SearchText { get; set; }
            public string FolderId { get; set; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotesBySearchTextAndFolderId"/> class.
        /// </summary>
        public NotesBySearchTextAndFolderId()
        {
            this.Map = notes => from note in notes
                                let folder = this.LoadDocument<Folder>(note.FolderId)
                                from parentFolder in this.Recurse(folder, f => this.LoadDocument<Folder>(f.ParentFolderId))
                                select new
                                           {
                                               SearchText = note.Tags.Concat(parentFolder.Tags).Concat(new[] { note.Name, note.Content, parentFolder.Name }),
                                               note.FolderId
                                           };

            this.Index(f => f.SearchText, FieldIndexing.Analyzed);
        }

        /// <summary>
        /// Gets the name of the index.
        /// </summary>
        public override string IndexName
        {
            get { return "Notes/BySearchTextAndFolderId"; }
        }
    }
}
