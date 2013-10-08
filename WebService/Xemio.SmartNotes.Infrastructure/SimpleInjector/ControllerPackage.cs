using System.Linq;
using SimpleInjector;
using SimpleInjector.Packaging;
using Xemio.SmartNotes.Infrastructure.Controllers;

namespace Xemio.SmartNotes.Infrastructure.SimpleInjector
{
    public class ControllerPackage : IPackage
    {
        #region Implementation of IPackage
        /// <summary>
        /// Registers the set of services in the specified <paramref name="container"/>.
        /// </summary>
        /// <param name="container">The container the set of services is registered into.</param>
        public void RegisterServices(Container container)
        {
            var controllers = from type in typeof(ControllerPackage).Assembly.GetExportedTypes()
                              where typeof(BaseController).IsAssignableFrom(type)
                              select type;

            container.RegisterAll(controllers);
        }
        #endregion
    }
}
