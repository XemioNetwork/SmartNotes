using System.Linq;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;
using Xemio.SmartNotes.Shared.Entities.Notes;

namespace Xemio.SmartNotes.Server.Infrastructure.RavenDB.Indexes
{
    /// <summary>
    /// Enables suggestions for <see cref="Note"/>s.
    /// </summary>
    public class NotesBySearchTextAndUserIdForSuggestions : AbstractIndexCreationTask<Note, NotesBySearchTextAndUserIdForSuggestions.Result>
    {
        /// <summary>
        /// The result of the <see cref="NotesBySearchTextAndUserIdForSuggestions"/> index.
        /// </summary>
        public class Result
        {
            public string[] SearchText { get; set; }
            public string UserId { get; set; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotesBySearchTextAndUserIdForSuggestions"/> class.
        /// </summary>
        public NotesBySearchTextAndUserIdForSuggestions()
        {
            this.Map = notes => from note in notes
                                let folder = this.LoadDocument<Folder>(note.FolderId)
                                from parentFolder in this.Recurse(folder, f => this.LoadDocument<Folder>(f.ParentFolderId))
                                select new
                                {
                                    SearchText = note.Tags.Concat(parentFolder.Tags).Concat(new[] { note.Name, note.Content, parentFolder.Name }),
                                    note.UserId
                                };

            this.Index(f => f.SearchText, FieldIndexing.Analyzed);
        }

        /// <summary>
        /// Gets the name of the index.
        /// </summary>
        public override string IndexName
        {
            get { return "Notes/BySearchTextAndUserId/Suggestions"; }
        }
    }
}
