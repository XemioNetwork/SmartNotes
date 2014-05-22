using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CuttingEdge.Conditions;
using Xemio.SmartNotes.Server.Abstractions.Security;

namespace Xemio.SmartNotes.Server.Infrastructure.Implementations.Security
{
    public class SaltCombiner : ISaltCombiner
    {
        #region Implementation of ISaltCombiner
        /// <summary>
        /// Combines the specified <paramref name="salt"/> with the specified <paramref name="password"/>.
        /// </summary>
        /// <param name="salt">The salt.</param>
        /// <param name="password">The password.</param>
        public byte[] Combine(byte[] salt, string password)
        {
            Condition.Requires(password, "password")
                .IsNotNull();

            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

            //Append the password to the salt
            List<byte> combined = salt.ToList();
            combined.AddRange(passwordBytes);

            //Hash the password
            return SHA256.Create().ComputeHash(combined.ToArray());
        }
        #endregion
    }
}
