using System;
using System.Linq;

namespace Xemio.SmartNotes.Shared.Extensions
{
    /// <summary>
    /// Contains extension methods for the <see cref="String"/> class.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Gets the int identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public static int GetIntId(this string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentOutOfRangeException("id");
            
            return int.Parse(id.Split('/').Last());
        }
    }
}
