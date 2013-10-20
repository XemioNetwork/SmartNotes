using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;

namespace Xemio.SmartNotes.Infrastructure.Windsor
{
    public class RavenInstaller : IWindsorInstaller
    {
        #region Implementation of IWindsorInstaller
        /// <summary>
        /// Performs the installation in the <see cref="T:Castle.Windsor.IWindsorContainer"/>.
        /// </summary>
        /// <param name="container">The container.</param><param name="store">The configuration store.</param>
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register
            (
                Component.For<IDocumentStore>().Instance(this.GetDocumentStore()).LifestyleSingleton(),
                Component.For<IAsyncDocumentSession>().UsingFactoryMethod((kernel, context) => kernel.Resolve<IDocumentStore>().OpenAsyncSession()).LifestyleTransient()
            );
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
