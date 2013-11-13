using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Xemio.SmartNotes.Models.Entities.Notes;
using Xemio.SmartNotes.Models.Entities.Users;

namespace Xemio.SmartNotes.Abstractions.Controllers
{
    public interface IFoldersController : IController
    {
        /// <summary>
        /// Returns all <see cref="Folder"/>s from the given <see cref="User"/>.
        /// </summary>
        /// <param name="userId">The user id.</param>
        Task<HttpResponseMessage> GetAllFolders(int userId);
        /// <summary>
        /// Creates a new folder.
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <param name="userId">The user id.</param>
        Task<HttpResponseMessage> PostFolder(Folder folder, int userId);
        /// <summary>
        /// Updates the folder.
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="folderId">The folder id.</param>
        Task<HttpResponseMessage> PutFolder(Folder folder, int userId, int folderId);
        /// <summary>
        /// Deletes the folder.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="folderId">The folder id.</param>
        Task<HttpResponseMessage> DeleteFolder(int userId, int folderId);
    }
}