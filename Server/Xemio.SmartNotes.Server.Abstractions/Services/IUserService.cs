using System.Threading.Tasks;
using Xemio.SmartNotes.Shared.Entities.Users;

namespace Xemio.SmartNotes.Server.Abstractions.Services
{
    /// <summary>
    /// Provides common methods for the <see cref="User"/> class.
    /// </summary>
    public interface IUserService : IService
    {
        /// <summary>
        /// Returns the current user.
        /// </summary>
        /// <param name="throwIfNoUser">If set to <c>true</c> an exception will be thrown if there is no current user.</param>
        User GetCurrentUser(bool throwIfNoUser = true);
    }
}
