using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xemio.SmartNotes.Entities.Users;

namespace Xemio.SmartNotes.Core.Services
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
