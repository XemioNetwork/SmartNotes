using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xemio.SmartNotes.Entities.Notes;
using Xemio.SmartNotes.Entities.Users;

namespace Xemio.SmartNotes.Core.Services
{
    /// <summary>
    /// Provides common methods for the <see cref="IUserSpecificEntity"/> class.
    /// </summary>
    public interface IRightsService : IService
    {
        /// <summary>
        /// Determines whether the current <see cref="User"/> has the given <paramref name="userId"/>.
        /// </summary>
        /// <param name="userId">The user id.</param>
        Task<bool> HasCurrentUserTheUserId(int userId);
        /// <summary>
        /// Determines whether the current <see cref="User"/> can cacess the <see cref="Note"/> with the given id.
        /// </summary>
        /// <param name="noteId">The note id.</param>
        Task<bool> CanCurrentUserAccessNote(int noteId);
        /// <summary>
        /// Determines whether the current <see cref="User"/> can cacess the <see cref="Note"/> with the given id.
        /// </summary>
        /// <param name="noteId">The note id.</param>
        Task<bool> CanCurrentUserAccessNote(string noteId);
        /// <summary>
        /// Determines whether the current <see cref="User"/> can access the <see cref="Folder"/> with the given id.
        /// </summary>
        /// <param name="folderId">The folder id.</param>
        Task<bool> CanCurrentUserAccessFolder(int folderId);
        /// <summary>
        /// Determines whether the current <see cref="User"/> can access the <see cref="Folder"/> with the given id.
        /// </summary>
        /// <param name="folderId">The folder id.</param>
        Task<bool> CanCurrentUserAccessFolder(string folderId);
    }
}
