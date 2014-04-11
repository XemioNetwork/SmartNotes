using System;
using System.ComponentModel.Composition.Hosting;
using System.Configuration;
using Castle.MicroKernel;
using Castle.MicroKernel.Context;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Raven.Bundles.CascadeDelete;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Embedded;
using Raven.Client.Indexes;
using Raven.Client.Listeners;
using Xemio.RavenDB.NGramAnalyzer;
using Xemio.SmartNotes.Server.Infrastructure.Extensions;
using Xemio.SmartNotes.Server.Infrastructure.RavenDB.Listeners;
using Xemio.SmartNotes.Shared.Entities.Notes;

namespace Xemio.SmartNotes.Server.Infrastructure.Windsor
{
    /// <summary>
    /// Installs RavenDB.
    /// </summary>
    public class RavenInstaller : IWindsorInstaller
    {
        #region Constants
        private const string ConnectionStringName = "RavenDB";
        #endregion

        #region Implementation of IWindsorInstaller
        /// <summary>
        /// Performs the installation in the <see cref="T:Castle.Windsor.IWindsorContainer"/>.
        /// </summary>
        /// <param name="container">The container.</param><param name="store">The configuration store.</param>
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register
            (
                Component.For<IDocumentStore>().UsingFactoryMethod(this.GetDocumentStore).LifestyleSingleton(),
                Component.For<IDocumentSession>().UsingFactoryMethod((kernel, context) => kernel.Resolve<IDocumentStore>().OpenSession()).LifestyleScoped()
            );
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Creates the document store and all indexes.
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        /// <param name="context">The context.</param>
        private IDocumentStore GetDocumentStore(IKernel kernel, CreationContext context)
        {
            DocumentStore documentStore = this.CreateDocumentStore();
            documentStore.RegisterMultipleListeners(new NoteCascadeDeleteListener(documentStore));
            documentStore.RegisterMultipleListeners(new FolderCascadeDeleteListener(documentStore));

            documentStore.Initialize();

            IndexCreation.CreateIndexes(this.GetType().Assembly, documentStore);

            return documentStore;
        }

        /// <summary>
        /// Creates the document store depending on the connection string.
        /// </summary>
        private DocumentStore CreateDocumentStore()
        {
            if (this.HasConnectionString() == false)
                throw new ConfigurationErrorsException(string.Format("No connection string for RavenDB was found. {0}Make sure you have a 'RavenDB' connection string in the app.config", Environment.NewLine));

            //We support the RavenDB embedded version
            if (this.IsEmbeddedConnectionString())
            {
                var store = new EmbeddableDocumentStore
                {
                    ConnectionStringName = ConnectionStringName,
                    UseEmbeddedHttpServer = true
                }; 

                var catalog = new AggregateCatalog();
                catalog.Catalogs.Add(new AssemblyCatalog(typeof(CascadeDeleteTrigger).Assembly));
                catalog.Catalogs.Add(new AssemblyCatalog(typeof(NGramAnalyzer).Assembly));
                //Add other catalogs here
                store.Configuration.Catalog.Catalogs.Add(catalog);
                
                return store;
            }
            else 
            { 
                return new DocumentStore
                {
                    ConnectionStringName = ConnectionStringName,
                    DefaultDatabase = "XemioNotes"
                };
            }
        }
        /// <summary>
        /// Determines whether we have a configured RavenDB connection string.
        /// </summary>
        private bool HasConnectionString()
        {
            return ConfigurationManager.ConnectionStrings[ConnectionStringName] != null;
        }
        /// <summary>
        /// Determines whether the RavenDB connection string is for a embedded document store.
        /// </summary>
        private bool IsEmbeddedConnectionString()
        {
            var connectionString = ConfigurationManager.ConnectionStrings[ConnectionStringName].ConnectionString.ToLower();

            return connectionString.Contains("datadir") && 
                   connectionString.Contains("url") == false;
        }
        #endregion
    }
}
