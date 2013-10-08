using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;
using SimpleInjector;
using SimpleInjector.Extensions.LifetimeScoping;

namespace Xemio.SmartNotes.Infrastructure.SimpleInjector.Extensions
{
    public class SimpleInjectorDependencyResolver : IDependencyResolver
    {
        #region Fields
        private readonly Container _container;
        private readonly LifetimeScope _lifetimeScope;
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleInjectorDependencyResolver"/> class.
        /// </summary>
        /// <param name="container">The _container.</param>
        public SimpleInjectorDependencyResolver(Container container)
            : this(container, false)
        {
        }
        /// <summary>
        /// Prevents a default instance of the <see cref="SimpleInjectorDependencyResolver"/> class from being created.
        /// </summary>
        /// <param name="container">The _container.</param>
        /// <param name="createScope">if set to <c>true</c> [create scope].</param>
        private SimpleInjectorDependencyResolver(Container container, bool createScope)
        {
            this._container = container;

            if (createScope)
            {
                this._lifetimeScope = container.BeginLifetimeScope();
            }
        }
        /// <summary>
        /// Starts a resolution scope.
        /// </summary>
        public IDependencyScope BeginScope()
        {
            return new SimpleInjectorDependencyResolver(this._container, true);
        }
        /// <summary>
        /// Retrieves a service from the scope.
        /// </summary>
        /// <param name="serviceType">The service to be retrieved.</param>
        public object GetService(Type serviceType)
        {
            return ((IServiceProvider)this._container)
                .GetService(serviceType);
        }
        /// <summary>
        /// Retrieves a collection of services from the scope.
        /// </summary>
        /// <param name="serviceType">The collection of services to be retrieved.</param>
        public IEnumerable<object> GetServices(Type serviceType)
        {
            return this._container.GetAllInstances(serviceType);
        }
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (this._lifetimeScope != null)
            {
                this._lifetimeScope.Dispose();
            }
        }
    }
}
