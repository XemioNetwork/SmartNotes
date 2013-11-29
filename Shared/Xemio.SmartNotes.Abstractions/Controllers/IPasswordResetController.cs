using System.Net.Http;
using System.Threading.Tasks;
using Xemio.SmartNotes.Models.Entities.Users;
using Xemio.SmartNotes.Models.Models;

namespace Xemio.SmartNotes.Abstractions.Controllers
{
    public interface IPasswordResetController : IController
    {
        /// <summary>
        /// Creates a new <see cref="PasswordReset"/>.
        /// </summary>
        /// <param name="data">The username or the email address of the user.</param>
        Task<HttpResponseMessage> PostPasswordReset(CreatePasswordReset data);

        /// <summary>
        /// Finishes a password reset.
        /// </summary>
        /// <param name="secret">The secret.</param>
        Task<HttpResponseMessage> GetPasswordReset(string secret);
    }
}