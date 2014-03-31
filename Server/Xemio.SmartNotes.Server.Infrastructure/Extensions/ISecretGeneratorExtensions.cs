using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xemio.SmartNotes.Server.Abstractions.Security;

namespace Xemio.SmartNotes.Server.Infrastructure.Extensions
{
    public static class ISecretGeneratorExtensions
    {
        public static string GenerateString(this ISecretGenerator generator, int length = 128)
        {
            if (length % 4 != 0)
                throw new ArgumentException("Must be divisible with 4.", "length");

            byte[] randomData =  generator.Generate(length/4*3);
            return Convert.ToBase64String(randomData).Replace('/', '-').Replace('+', '_');
        }
    }
}
