using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Metadata.Providers;
using CuttingEdge.Conditions;
using Xemio.SmartNotes.Server.Abstractions.Security;

namespace Xemio.SmartNotes.Server.Infrastructure.Implementations.Security
{
    public class SecretGenerator : ISecretGenerator
    {
        #region Implementation of ISecretGenerator
        /// <summary>
        /// Generates a new secret.
        /// </summary>
        public byte[] Generate(int length = 128)
        {
            Condition.Requires(length, "length")
                .IsNotLessOrEqual(0);

            byte[] randomBytes = new byte[length];
            RandomNumberGenerator.Create().GetNonZeroBytes(randomBytes);

            return randomBytes;
        }
        #endregion
    }
}
