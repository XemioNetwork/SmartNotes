using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Metadata.Providers;
using Xemio.SmartNotes.Server.Abstractions.Security;

namespace Xemio.SmartNotes.Server.Infrastructure.Implementations.Security
{
    public class SecretGenerator : ISecretGenerator
    {
        #region Implementation of ISecretGenerator
        /// <summary>
        /// Generates a new secret.
        /// </summary>
        public string Generate(int length = 32)
        {
            byte[] randomBytes = new byte[length / 4 * 3];
            RandomNumberGenerator.Create().GetNonZeroBytes(randomBytes);
            return Convert.ToBase64String(randomBytes).Replace('/', '-').Replace('+', '_');
        }
        #endregion
    }
}
