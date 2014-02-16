using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xemio.SmartNotes.Server.Abstractions.Services;

namespace Xemio.SmartNotes.Server.Infrastructure.Implementations.Services
{
    public class FileService : IFileService
    {
        #region Implementation of IFileService
        /// <summary>
        /// Gets the full path.
        /// </summary>
        /// <param name="subPath">The sub path.</param>
        public string GetFullPath(string subPath)
        {
            return Path.Combine(Environment.CurrentDirectory, subPath);
        }
        #endregion
    }
}
