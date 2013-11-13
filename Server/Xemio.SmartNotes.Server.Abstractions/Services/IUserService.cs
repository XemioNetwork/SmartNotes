using System.Threading.Tasks;
using Xemio.SmartNotes.Models.Entities.Users;

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
        Task<User> GetCurrentUser();
    }
}
