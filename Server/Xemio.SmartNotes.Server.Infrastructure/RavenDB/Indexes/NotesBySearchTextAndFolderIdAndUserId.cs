﻿using System;
using System.Linq;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;
using Xemio.SmartNotes.Shared.Entities.Notes;

namespace Xemio.SmartNotes.Server.Infrastructure.RavenDB.Indexes
{
    /// <summary>
    /// Enables fulltext search for <see cref="Note"/>s.
    /// </summary>
    public class NotesBySearchTextAndFolderIdAndUserId : AbstractIndexCreationTask<Note, NotesBySearchTextAndFolderIdAndUserId.Result>
    {
        /// <summary>
        /// The result of the <see cref="NotesBySearchTextAndFolderIdAndUserId"/> index.
        /// </summary>
        public class Result
        {
            public string[] SearchText { get; set; }
            public string FolderId { get; set; }
            public string UserId { get; set; }
            public string Name { get; set; }
            public DateTimeOffset CreatedDate { get; set; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotesBySearchTextAndFolderIdAndUserId"/> class.
        /// </summary>
        public NotesBySearchTextAndFolderIdAndUserId()
        {
            this.Map = notes => from note in notes
                                let folder = this.LoadDocument<Folder>(note.FolderId)
                                from parentFolder in this.Recurse(folder, f => this.LoadDocument<Folder>(f.ParentFolderId))
                                select new
                                           {
                                               SearchText = note.Tags.Concat(parentFolder.Tags).Concat(new[] { note.Name, note.Content, parentFolder.Name }),
                                               note.FolderId,
                                               note.UserId,
                                               note.Name,
                                               note.CreatedDate
                                           };

            this.Index(f => f.SearchText, FieldIndexing.Analyzed);
            this.Analyze(f => f.SearchText, "Xemio.RavenDB.NGramAnalyzer.NGramAnalyzer, Xemio.RavenDB.NGramAnalyzer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
        }

        /// <summary>
        /// Gets the name of the index.
        /// </summary>
        public override string IndexName
        {
            get { return "Notes/BySearchTextAndFolderIdAndUserId"; }
        }
    }
}
