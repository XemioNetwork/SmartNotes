using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raven.Bundles.CascadeDelete;
using Raven.Client;
using Raven.Json.Linq;

namespace Xemio.SmartNotes.Server.Infrastructure.Extensions
{
    /// <summary>
    /// Contains extension methods for the <see cref="ISyncAdvancedSessionOperation"/> interface.
    /// </summary>
    public static class ISyncAdvancedSessionOperationExtensions
    {
        /// <summary>
        /// Gets the string id for the given type.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="advanced">The advanced session operations.</param>
        /// <param name="id">The id.</param>
        public static string GetStringIdFor<T>(this ISyncAdvancedSessionOperation advanced, int id)
        {
            return advanced.DocumentStore.Conventions.FindFullDocumentKeyFromNonStringIdentifier(id, typeof(T), false);
        }
        /// <summary>
        /// Adds the cascade delete for the specified <paramref name="documentIds"/>.
        /// </summary>
        /// <param name="advanced">The advanced session operations.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="documentIds">The document keys.</param>
        public static void AddCascadeDelete(this ISyncAdvancedSessionOperation advanced, object entity, params string[] documentIds)
        {
            if (documentIds.Length == 0)
                throw new ArgumentException("At least one document id must be specified.");

            ExecuteOnMetadataKey(advanced, MetadataKeys.DocumentsToCascadeDelete, entity, list =>
            {
                foreach (string item in documentIds.Where(f => !list.Contains(f)))
                    list.Add(item);
            });
        }
        /// <summary>
        /// Removes the cascade delete for the specified <paramref name="documentIds"/>.
        /// </summary>
        /// <param name="advanced">The advanced.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="documentIds">The document keys.</param>
        public static void RemoveCascadeDelete(this ISyncAdvancedSessionOperation advanced, object entity, params string[] documentIds)
        {
            if (documentIds.Length == 0)
                throw new ArgumentException("At least one document id must be specified.");

            ExecuteOnMetadataKey(advanced, MetadataKeys.DocumentsToCascadeDelete, entity, list =>
            {
                foreach (string item in documentIds.Where(f => list.Contains(f)))
                    list.Remove(item);
            });
        }
        /// <summary>
        /// Adds the cascade delete for the specified <paramref name="attachmentIds"/>
        /// </summary>
        /// <param name="advanced">The advanced.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="attachmentIds">The attachment ids.</param>
        public static void AddCascadeDeleteAttachment(this ISyncAdvancedSessionOperation advanced, object entity, params string[] attachmentIds)
        {
            if (attachmentIds.Length == 0)
                throw new ArgumentException("At least one attachment id must be specified.");

            ExecuteOnMetadataKey(advanced, MetadataKeys.AttachmentsToCascadeDelete, entity, list =>
            {
                foreach(string item in attachmentIds.Where(f => !list.Contains(f)))
                    list.Add(item);
            });
        }
        /// <summary>
        /// Removes the cascade delete for the specified <paramref name="attachmentIds"/>.
        /// </summary>
        /// <param name="advanced">The advanced.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="attachmentIds">The attachment ids.</param>
        public static void RemoveCascadeDeleteAttachment(this ISyncAdvancedSessionOperation advanced, object entity, params string[] attachmentIds)
        {
            if (attachmentIds.Length == 0)
                throw new ArgumentException("At least one attachment id must be specified.");

            ExecuteOnMetadataKey(advanced, MetadataKeys.AttachmentsToCascadeDelete, entity, list =>
            {
                foreach (string item in attachmentIds.Where(f => list.Contains(f)))
                    list.Remove(item);
            });
        }
        /// <summary>
        /// Executes the <paramref name="execute"/> with the <see cref="RavenJArray"/> from the metadata.
        /// </summary>
        /// <param name="advanced">The advanced.</param>
        /// <param name="metadataKey">The metadata key.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="execute">The execute.</param>
        private static void ExecuteOnMetadataKey(ISyncAdvancedSessionOperation advanced, string metadataKey, object entity, Action<RavenJArray> execute)
        {
            RavenJObject metadata = advanced.GetMetadataFor(entity);

            if (metadata == null)
                throw new InvalidOperationException("The entity must be tracked in the session before calling this method.");
            
            RavenJToken token;
            if (!metadata.TryGetValue(metadataKey, out token))
                token = new RavenJArray();

            var list = (RavenJArray)token;
            execute(list);
            
            metadata[metadataKey] = list;
        }
    }
}
