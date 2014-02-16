using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xemio.SmartNotes.Shared.Extensions
{
    public static class ICollectionExtensions
    {
        /// <summary>
        /// Adds all items to the specified <paramref name="collection"/>.
        /// </summary>
        /// <typeparam name="T">The item type.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="items">The items.</param>
        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                collection.Add(item);
            }
        }
    }
}
