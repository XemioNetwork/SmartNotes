using Castle.Core;
using CuttingEdge.Conditions;
using Raven.Abstractions.Data;
using Raven.Client;
using Xemio.SmartNotes.Server.Infrastructure.RavenDB.Indexes;

namespace Xemio.SmartNotes.Server.Infrastructure.Implementations.Startables
{
    public class FolderNoteCountScriptsInstaller : IStartable
    {
        #region Fields
        private readonly IDocumentStore _documentStore;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="FolderNoteCountScriptsInstaller"/> class.
        /// </summary>
        /// <param name="documentStore">The document store.</param>
        public FolderNoteCountScriptsInstaller(IDocumentStore documentStore)
        {
            Condition.Requires(documentStore, "documentStore")
                .IsNotNull();

            this._documentStore = documentStore;
        }
        #endregion

        #region Implementation of IStartable
        /// <summary>
        /// Executed on application start.
        /// </summary>
        public void Start()
        {
            using (IDocumentSession session = this._documentStore.OpenSession())
            {
                string id = ScriptedIndexResults.IdPrefix + new NotesByFolderNoteCount().IndexName;
                var document = new ScriptedIndexResults
                {
                    Id = id,
                    IndexScript = "var folder = LoadDocument(this.FolderId); " +
                                  "if(folder == null) " +
                                      "return; " +
                                  "if (folder.NoteCount == this.Count) " +
                                      "return; " +
                                  "folder.NoteCount = this.Count; " +
                                  "PutDocument(this.FolderId, folder);",

                    DeleteScript = "var folder = LoadDocument(key); " +
                                   "if(folder == null) " +
                                       "return; " +
                                   "delete folder.NoteCount; " +
                                   "PutDocument(key, folder);"
                };

                session.Store(document);

                session.SaveChanges();
            }
        }
        /// <summary>
        /// Executed on application shutdown.
        /// </summary>
        public void Stop()
        {
        }
        #endregion
    }
}