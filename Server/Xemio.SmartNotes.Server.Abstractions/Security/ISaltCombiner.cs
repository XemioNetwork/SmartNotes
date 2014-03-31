using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xemio.SmartNotes.Server.Abstractions.Security
{
    public interface ISaltCombiner : IService
    {
        /// <summary>
        /// Combines the specified <paramref name="salt"/> with the specified <paramref name="password"/>.
        /// </summary>
        /// <param name="salt">The salt.</param>
        /// <param name="password">The password.</param>
        byte[] Combine(byte[] salt, string password);
    }
}
