﻿using System.Net.Http;
using System.Threading.Tasks;
using Xemio.SmartNotes.Models.Entities.Notes;
using Xemio.SmartNotes.Models.Entities.Users;

namespace Xemio.SmartNotes.Client.Abstractions.Clients
{
    public interface IFoldersClient : IClient
    {
        /// <summary>
        /// Returns all <see cref="Folder"/>s from the given <see cref="User"/>.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="parentFolderId">The parent folder id.</param>
        Task<HttpResponseMessage> GetAllFolders(int userId, string parentFolderId);

        /// <summary>
        /// Creates a new <see cref="Folder"/>.
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <param name="userId">The user id.</param>
        Task<HttpResponseMessage> PostFolder(Folder folder, int userId);

        /// <summary>
        /// Updates the <see cref="Folder"/>.
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="folderId">The folder id.</param>
        Task<HttpResponseMessage> PutFolder(Folder folder, int userId, int folderId);

        /// <summary>
        /// Deletes the <see cref="Folder"/>.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="folderId">The folder id.</param>
        Task<HttpResponseMessage> DeleteFolder(int userId, int folderId);
    }
}