﻿using System;
using System.ComponentModel.Composition.Hosting;
using System.Configuration;
using System.Reflection.Emit;
using Castle.MicroKernel;
using Castle.MicroKernel.Context;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;
using Raven.Client.Listeners;
using Raven.Database.Server;
using Xemio.SmartNotes.Server.Infrastructure.Extensions;
using Xemio.SmartNotes.Server.Infrastructure.RavenDB.Listeners;
using Xemio.SmartNotes.Shared.Entities.Notes;
using Xemio.SmartNotes.Shared.Entities.Users;
using Xemio.SmartNotes.Shared.Helpers;

namespace Xemio.SmartNotes.Server.Infrastructure.Windsor
{
    /// <summary>
    /// Installs RavenDB.
    /// </summary>
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
            if (this.HasConnectionString() == false)
                throw new ConfigurationErrorsException(string.Format("No connection string for RavenDB was found. {0}Make sure you have a connection string in the app.config / web.config", Environment.NewLine));

            var documentStore = new DocumentStore
            {
                ConnectionStringName = Dependency.OnAppSettingsValue("XemioNotes/RavenConnectionStringName").Value,
                DefaultDatabase = Dependency.OnAppSettingsValue("XemioNotes/RavenDatabaseName").Value,
            };

            documentStore.RegisterMultipleListeners(new NoteCascadeDeleteListener(documentStore));
            documentStore.RegisterMultipleListeners(new FolderCascadeDeleteListener(documentStore));

            this.UpdateFindIdentityProperty(documentStore);

            documentStore.Initialize();

            IndexCreation.CreateIndexes(this.GetType().Assembly, documentStore);

            return documentStore;
        }
        /// <summary>
        /// Determines whether we have a configured RavenDB connection string.
        /// </summary>
        private bool HasConnectionString()
        {
            string connectionStringName = Dependency.OnAppSettingsValue("XemioNotes/RavenConnectionStringName").Value;
            return ConfigurationManager.ConnectionStrings[connectionStringName] != null;
        }
        /// <summary>
        /// Updates the find identity property.
        /// </summary>
        /// <param name="documentStore">The document store.</param>
        private void UpdateFindIdentityProperty(IDocumentStore documentStore)
        {
            documentStore.Conventions.RegisterIdConvention<FacebookAuthentication>((dbName, commands, entity) =>
            {
                string tag = documentStore.Conventions.GetTypeTagName(typeof(FacebookAuthentication));
                return string.Format("{0}/{1}", tag, entity.FacebookUserId);
            });
            documentStore.Conventions.RegisterIdConvention<CachedFacebookTokenExchange>((dbName, commands, entity) =>
            {
                string tag = documentStore.Conventions.GetTypeTagName(typeof(CachedFacebookTokenExchange));
                return string.Format("{0}/{1}", tag, entity.Token);
            });
            documentStore.Conventions.RegisterIdConvention<AuthenticationToken>((dbName, commands, entity) =>
            {
                string tag = documentStore.Conventions.GetTypeTagName(typeof(AuthenticationToken));
                return string.Format("{0}/{1}", tag, entity.Token);
            });
            documentStore.Conventions.RegisterIdConvention<XemioAuthentication>((dbName, commands, entity) =>
            {
                string tag = documentStore.Conventions.GetTypeTagName(typeof (XemioAuthentication));
                return string.Format("{0}/{1}", tag, entity.UserId);
            });
        }
        #endregion
    }
}
