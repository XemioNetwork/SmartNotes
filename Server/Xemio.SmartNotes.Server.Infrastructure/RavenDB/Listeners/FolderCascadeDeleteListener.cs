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
    internal class FolderCascadeDeleteListener : IDocumentStoreListener
    {
        #region Fields
        private readonly DocumentStore _documentStore;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="FolderCascadeDeleteListener"/> class.
        /// </summary>
        /// <param name="documentStore">The document store.</param>
        public FolderCascadeDeleteListener(DocumentStore documentStore)
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
            var folder = entityInstance as Folder;
            
            //Only handle folders
            if (folder == null)
                return false;

            using (IDocumentSession session = this._documentStore.OpenSession())
            {
                //Get the old parent folder id
                RavenJToken token;
                original.TryGetValue(ReflectionHelper.GetProperty<Folder>(f => f.ParentFolderId).Name, out token);

                string oldParentFolderId = token != null ? token.Value<string>() : null;

                //Check if the parent folder has changed
                if (folder.ParentFolderId == oldParentFolderId)
                    return false;

                //Add the cascade delete to the new parent folder
                if (string.IsNullOrWhiteSpace(folder.ParentFolderId) == false)
                {
                    var newParentFolder = session.Load<Folder>(folder.ParentFolderId);
                    session.Advanced.AddCascadeDelete(newParentFolder, folder.Id);
                }

                //Remove the cascade delete from the old parent folder
                if (oldParentFolderId != null)
                {
                    var oldParentFolder = session.Load<Folder>(oldParentFolderId);
                    session.Advanced.RemoveCascadeDelete(oldParentFolder, folder.Id);
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
    }
}
