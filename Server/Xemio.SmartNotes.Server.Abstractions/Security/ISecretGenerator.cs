using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xemio.SmartNotes.Server.Abstractions.Services;

namespace Xemio.SmartNotes.Server.Abstractions.Security
{
    public interface ISecretGenerator : IService
    {
        /// <summary>
        /// Generates a new secret.
        /// </summary>
        byte[] Generate(int length = 128);
    }
}
