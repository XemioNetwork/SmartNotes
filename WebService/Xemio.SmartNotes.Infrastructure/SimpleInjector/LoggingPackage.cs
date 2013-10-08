using NLog;
using SimpleInjector;
using SimpleInjector.Packaging;
using Xemio.SmartNotes.Infrastructure.SimpleInjector.Extensions;

namespace Xemio.SmartNotes.Infrastructure.SimpleInjector
{
    public class LoggingPackage : IPackage
    {
        #region Implementation of IPackage
        /// <summary>
        /// Registers the set of services in the specified <paramref name="container"/>.
        /// </summary>
        /// <param name="container">The container the set of services is registered into.</param>
        public void RegisterServices(Container container)
        {
            container.Register(() => new LogFactory());
            container.RegisterWithContext(context => LogManager.GetLogger(context.ImplementationType.FullName));
        }
        #endregion
    }
}
