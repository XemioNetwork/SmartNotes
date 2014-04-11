using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Listeners;
using Raven.Json.Linq;
using Xemio.SmartNotes.Server.Infrastructure.Extensions;
using Xemio.SmartNotes.Shared.Entities.Notes;
using Xemio.SmartNotes.Shared.Helpers;

namespace Xemio.SmartNotes.Server.Infrastructure.RavenDB.Listeners
{
    internal class NoteCascadeDeleteListener : IDocumentStoreListener, IDocumentDeleteListener
    {
        #region Fields
        private readonly DocumentStore _documentStore;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="NoteCascadeDeleteListener"/> class.
        /// </summary>
        /// <param name="documentStore">The document store.</param>
        public NoteCascadeDeleteListener(DocumentStore documentStore)
        {
            this._documentStore = documentStore;
        }
        #endregion

        #region Implementation of IDocumentStoreListener
        /// <summary>
        /// Invoked before the store request is sent to the server.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="entityInstance">The entity instance.</param>
        /// <param name="metadata">The metadata.</param>
        /// <param name="original">The original document that was loaded from the server</param>
        /// <returns>
        /// Whatever the entity instance was modified and requires us re-serialize it.
        /// Returning true would force re-serialization of the entity, returning false would
        /// mean that any changes to the entityInstance would be ignored in the current SaveChanges call.
        /// </returns>
        public bool BeforeStore(string key, object entityInstance, RavenJObject metadata, RavenJObject original)
        {
            var note = entityInstance as Note;
            
            //Only handle notes
            if (note == null)
                return false;

            using (IDocumentSession session = this._documentStore.OpenSession())
            {
                //Get the old folder id
                RavenJToken token;
                original.TryGetValue(ReflectionHelper.GetProperty<Note>(f => f.FolderId).Name, out token);

                string oldFolderId = token != null ? token.Value<string>() : null;

                //Check if it has changed
                if (oldFolderId == note.FolderId)
                    return false;

                //Add the cascade delete to the new folder
                var folder = session.Load<Folder>(note.FolderId);
                session.Advanced.AddCascadeDelete(folder, note.Id);
                
                //Remove the cascade delete from the old folder
                if (oldFolderId != null)
                { 
                    var oldFolder = session.Load<Folder>(oldFolderId);
                    session.Advanced.RemoveCascadeDelete(oldFolder, note.Id);
                }

                session.SaveChanges();
            }

            return false;
        }
        /// <summary>
        /// Invoked after the store request is sent to the server.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="entityInstance">The entity instance.</param>
        /// <param name="metadata">The metadata.</param>
        public void AfterStore(string key, object entityInstance, RavenJObject metadata)
        {
        }
        #endregion

        #region Implementation of IDocumentDeleteListener
        /// <summary>
        /// Invoked before the delete request is sent to the server.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="entityInstance">The entity instance.</param>
        /// <param name="metadata">The metadata.</param>
        public void BeforeDelete(string key, object entityInstance, RavenJObject metadata)
        {
            var note = entityInstance as Note;

            if (note == null)
                return;

            using (var session = this._documentStore.OpenSession())
            {
                //Remove the cascade delete from the folder
                var folder = session.Load<Folder>(note.FolderId);
                session.Advanced.RemoveCascadeDelete(folder, note.Id);

                session.SaveChanges();
            }
        }
        #endregion
    }
}
