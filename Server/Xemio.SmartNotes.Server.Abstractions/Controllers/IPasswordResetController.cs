using System.Net.Http;
using Xemio.SmartNotes.Models.Entities.Users;
using Xemio.SmartNotes.Models.Models;

namespace Xemio.SmartNotes.Server.Abstractions.Controllers
{
    public interface IPasswordResetController : IController
    {
        /// <summary>
        /// Creates a new <see cref="PasswordReset"/>.
        /// </summary>
        /// <param name="data">The username or the email address of the user.</param>
        HttpResponseMessage PostPasswordReset(CreatePasswordReset data);

        /// <summary>
        /// Finishes a password reset.
        /// </summary>
        /// <param name="secret">The secret.</param>
        HttpResponseMessage GetPasswordReset(string secret);
    }
}