using System.Linq;
using SimpleInjector;
using SimpleInjector.Packaging;
using Xemio.SmartNotes.Core.Services;

namespace Xemio.SmartNotes.Infrastructure.SimpleInjector
{
    public class ServicePackage : IPackage
    {
        #region Implementation of IPackage
        /// <summary>
        /// Registers the set of services in the specified <paramref name="container"/>.
        /// </summary>
        /// <param name="container">The container the set of services is registered into.</param>
        public void RegisterServices(Container container)
        {
            var services = from type in typeof(ServicePackage).Assembly.GetExportedTypes()
                           where typeof(IService).IsAssignableFrom(type)
                           where type.IsInterface == false
                           select new
                                      {
                                          Service = type.GetInterfaces().First(),
                                          Implementation = type
                                      };

            foreach (var service in services)
            {
                container.Register(service.Service, service.Implementation);
            }
        }
        #endregion
    }
}
