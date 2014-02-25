using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Xemio.SmartNotes.Client.Windows.Extensions
{
    public static class ResourceDictionaryExtensions
    {
        #region Methods
        /// <summary>
        /// Returns the named resource.
        /// </summary>
        /// <typeparam name="T">The type of the resource.</typeparam>
        /// <param name="dictionary">The resource dictionary.</param>
        /// <param name="resourceName">The name of the resource.</param>
        public static T GetNamedResource<T>(this ResourceDictionary dictionary, [CallerMemberName]string resourceName = null)
        {
            if (resourceName == null)
                return default(T);

            return (T)dictionary[resourceName.MakeFirstCharLowerCase()];
        }
        #endregion
    }
}
