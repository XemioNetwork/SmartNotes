using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raven.Client;
using Raven.Json.Linq;

namespace Xemio.SmartNotes.Infrastructure.Extensions
{
    /// <summary>
    /// Contains extension methods for the <see cref="IAsyncDocumentSession"/> interface.
    /// </summary>
    public static class IAsyncAdvancedSessionOperationsExtensions
    {
        /// <summary>
        /// The metadata key for the cascade delete option.
        /// </summary>
        private const string CascadeMetadataKey = "Raven-Cascade-Delete-Documents";

        /// <summary>
        /// Gets the string id for the given type.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="advanced">The advanced session operations.</param>
        /// <param name="id">The id.</param>
        public static string GetStringIdFor<T>(this IAsyncAdvancedSessionOperations advanced, int id)
        {
            return advanced.DocumentStore.Conventions.FindFullDocumentKeyFromNonStringIdentifier(id, typeof(T), false);
        }
        /// <summary>
        /// Gets the int id from from the given <paramref name="id"/>.
        /// </summary>
        /// <param name="advanced">The advanced.</param>
        /// <param name="id">The id.</param>
        public static string GetIntIdFrom(this IAsyncAdvancedSessionOperations advanced, string id)
        {
            return id.Split('/').Last();
        }
        /// <summary>
        /// Adds a cascade delete to the <paramref name="entity"/>.
        /// </summary>
        /// <param name="advanced">The advanced session operations.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="documentKeys">The document keys.</param>
        public static void AddCascadeDelete(this IAsyncAdvancedSessionOperations advanced, object entity, params string[] documentKeys)
        {
            RavenJObject metadata = advanced.GetMetadataFor(entity);

            if (metadata == null)
                throw new InvalidOperationException("The entity must be tracked in the session before calling this method.");

            if (documentKeys.Length == 0)
                throw new ArgumentException("At least one document key must be specified.");

            RavenJToken token;
            if (!metadata.TryGetValue(CascadeMetadataKey, out token))
                token = new RavenJArray();

            RavenJArray list = (RavenJArray)token;
            foreach (string documentKey in documentKeys.Where(key => !list.Contains(key)))
                list.Add(documentKey);

            metadata[CascadeMetadataKey] = list;
        }
        /// <summary>
        /// Removes the cascade delete from the <paramref name="entity"/>.
        /// </summary>
        /// <param name="advanced">The advanced.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="documentKeys">The document keys.</param>
        public static void RemoveCascadeDelete(this IAsyncAdvancedSessionOperations advanced, object entity, params string[] documentKeys)
        {
            RavenJObject metadata = advanced.GetMetadataFor(entity);

            if (metadata == null)
                throw new InvalidOperationException("The entity must be tracked in the session before calling this method.");

            if (documentKeys.Length == 0)
                throw new ArgumentException("At least one document key must be specified.");

            RavenJToken token;
            if (!metadata.TryGetValue(CascadeMetadataKey, out token))
                token = new RavenJArray();

            RavenJArray list = (RavenJArray)token;
            foreach (string documentKey in documentKeys.Where(key => list.Contains(key)))
                list.Remove(documentKey);

            metadata[CascadeMetadataKey] = list;
        }
    }
}
