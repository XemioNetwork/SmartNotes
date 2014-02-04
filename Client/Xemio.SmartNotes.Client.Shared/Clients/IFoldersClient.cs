using System.Net.Http;
using System.Threading.Tasks;
using Xemio.SmartNotes.Shared.Entities.Notes;
using Xemio.SmartNotes.Shared.Entities.Users;

namespace Xemio.SmartNotes.Client.Shared.Clients
{
    public interface IFoldersClient : IClient
    {
        /// <summary>
        /// Returns all <see cref="Folder"/>s from the given <see cref="User"/>.
        /// </summary>
        /// <param name="parentFolderId">The parent folder id.</param>
        Task<HttpResponseMessage> GetAllFolders(string parentFolderId);

        /// <summary>
        /// Creates a new <see cref="Folder"/>.
        /// </summary>
        /// <param name="folder">The folder.</param>
        Task<HttpResponseMessage> PostFolder(Folder folder);

        /// <summary>
        /// Updates the <see cref="Folder"/>.
        /// </summary>
        /// <param name="folder">The folder.</param>
        Task<HttpResponseMessage> PutFolder(Folder folder);

        /// <summary>
        /// Deletes the <see cref="Folder"/>.
        /// </summary>
        /// <param name="folderId">The folder id.</param>
        Task<HttpResponseMessage> DeleteFolder(string folderId);
    }
}