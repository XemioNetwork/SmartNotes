using System.Net.Http;
using System.Threading.Tasks;
using Xemio.SmartNotes.Shared.Entities.Users;
using Xemio.SmartNotes.Shared.Models;

namespace Xemio.SmartNotes.Client.Shared.Clients
{
    public interface IPasswordResetClient : IClient
    {
        /// <summary>
        /// Creates a new <see cref="PasswordReset" />.
        /// </summary>
        /// <param name="data">The username or the email address of the user.</param>
        Task<HttpResponseMessage> PostPasswordReset(CreatePasswordReset data);
    }
}