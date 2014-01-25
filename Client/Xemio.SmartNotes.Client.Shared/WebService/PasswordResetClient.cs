using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xemio.SmartNotes.Client.Abstractions.Clients;
using Xemio.SmartNotes.Models.Entities.Users;
using Xemio.SmartNotes.Models.Models;

namespace Xemio.SmartNotes.Client.Shared.WebService
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
