using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;
using SimpleInjector;
using SimpleInjector.Packaging;

namespace Xemio.SmartNotes.Infrastructure.SimpleInjector
{
    public class RavenPackage : IPackage
    {
        #region Implementation of IPackage
        /// <summary>
        /// Registers the set of services in the specified <paramref name="container"/>.
        /// </summary>
        /// <param name="container">The container the set of services is registered into.</param>
        public void RegisterServices(Container container)
        {
            container.RegisterSingle(this.GetDocumentStore);
            container.RegisterLifetimeScope(() => container.GetInstance<IDocumentStore>().OpenAsyncSession());
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Creates the document store and all indexes.
        /// </summary>
        private IDocumentStore GetDocumentStore()
        {
            IDocumentStore documentStore = new DocumentStore
            {
                Url = "http://localhost:8080",
                DefaultDatabase = "SmartNotes"
            }.Initialize();

            IndexCreation.CreateIndexes(this.GetType().Assembly, documentStore);

            return documentStore;
        }
        #endregion
    }
}
