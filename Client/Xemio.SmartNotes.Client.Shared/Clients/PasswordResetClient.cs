using System.Net.Http;
using System.Threading.Tasks;
using Xemio.SmartNotes.Shared.Entities.Users;
using Xemio.SmartNotes.Shared.Models;

namespace Xemio.SmartNotes.Client.Shared.Clients
{
    public class PasswordResetClient : BaseClient, IPasswordResetClient
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="PasswordResetClient"/> class.
        /// </summary>
        /// <param name="baseAddress">The base address.</param>
        /// <param name="session">The application session.</param>
        public PasswordResetClient(string baseAddress, Session session)
            : base(baseAddress, session)
        {
        }
        #endregion

        #region Implementation of IPasswordResetController
        /// <summary>
        /// Creates a new <see cref="PasswordReset" />.
        /// </summary>
        /// <param name="data">The username or the email address of the user.</param>
        public Task<HttpResponseMessage> PostPasswordReset(CreatePasswordReset data)
        {
            var request = this.CreateRequest(HttpMethod.Post, "PasswordResets", data);
            return this.SendAsync(request);
        }
        #endregion
    }
}
