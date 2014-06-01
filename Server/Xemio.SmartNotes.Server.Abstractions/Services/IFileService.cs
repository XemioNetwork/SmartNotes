using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xemio.SmartNotes.Server.Abstractions.Services
{
    public interface IFileService : IService
    {
        /// <summary>
        /// Returns the full path.
        /// </summary>
        /// <param name="subPath">The sub path.</param>
        string GetFullPath(string subPath);

        /// <summary>
        /// Opens the file.
        /// </summary>
        /// <param name="path">The path.</param>
        Stream OpenFile(string path);
    }
}
