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
                Component.For<IDocumentSession>().UsingFactoryMethod((kernel, context) => kernel.Resolve<IDocumentStore>().OpenSession()).LifestyleTransient()
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
            IDocumentStore documentStore = this.CreateDocumentStore();

            IndexCreation.CreateIndexes(this.GetType().Assembly, documentStore);

            return documentStore;
        }
        /// <summary>
        /// Creates the document store depending on the connection string.
        /// </summary>
        private IDocumentStore CreateDocumentStore()
        {
            if (this.HasConnectionString() == false)
                throw new ConfigurationErrorsException(string.Format("No connection string for RavenDB was found. {0}Make sure you have a 'RavenDB' connection string in the app.config", Environment.NewLine));

            if (this.IsEmbeddedConnectionString())
            {
                var store = new EmbeddableDocumentStore
                {
                    ConnectionStringName = ConnectionStringName,
                    UseEmbeddedHttpServer = true
                };

                store.Initialize();

                var catalogs = store.DocumentDatabase.Configuration.Catalog.Catalogs;
                catalogs.Add(new AssemblyCatalog(typeof(CascadeDeleteTrigger).Assembly));

                return store;
            }

            return new DocumentStore
            {
                ConnectionStringName = ConnectionStringName,
                DefaultDatabase = "XemioNotes"
            }.Initialize();
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

            return connectionString.Contains("datadir") && connectionString.Contains("url") == false;
        }
        #endregion
    }
}
