using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Xemio.SmartNotes.Abstractions.Extensions
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
