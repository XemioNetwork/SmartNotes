using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Xemio.CommonLibrary.Security;
using Xemio.CommonLibrary.Storage;
using Xemio.CommonLibrary.Storage.Files;

namespace Xemio.SmartNotes.Client.Windows.Windsor
{
    public class DataStorageInstaller : IWindsorInstaller
    {
        #region Implementation of IWindsorInstaller
        /// <summary>
        /// Performs the installation in the <see cref="T:Castle.Windsor.IWindsorContainer" />.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="store">The configuration store.</param>
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register
            (
                Component.For<IDataStorage>().UsingFactoryMethod(GetDataStorage).LifestyleSingleton()
            );
        }
        /// <summary>
        /// Creates the data storage.
        /// </summary>
        private IDataStorage GetDataStorage()
        {
            return new DataStorage(new DataStorageSettings
            {
                FileSystem = new FileSystem(),
                Encrypter = new RijndaelEncryptor(Dependency.OnAppSettingsValue("password", "XemioNotes/LocalDataStoragePassword").Value)
            });
        }
        #endregion
    }
}
