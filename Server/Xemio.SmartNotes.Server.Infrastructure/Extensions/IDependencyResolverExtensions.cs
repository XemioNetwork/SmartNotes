using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Dependencies;

namespace Xemio.SmartNotes.Server.Infrastructure.Extensions
{
    /// <summary>
    /// Contains extension methods for the <see cref="IDependencyResolver"/> interface.
    /// </summary>
    public static class IDependencyResolverExtensions
    {
        /// <summary>
        /// Retrieves a service from the scope.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dependencyResolver">The dependency resolver.</param>
        public static T GetService<T>(this IDependencyResolver dependencyResolver)
        {
            return (T)dependencyResolver.GetService(typeof(T));
        }
    }
}
