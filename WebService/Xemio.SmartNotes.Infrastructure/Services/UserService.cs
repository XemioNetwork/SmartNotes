using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Raven.Client;
using Xemio.SmartNotes.Core.Services;
using Xemio.SmartNotes.Entities.Users;

namespace Xemio.SmartNotes.Infrastructure.Services
{
    public class UserService : IUserService
    {
        #region Fields
        private readonly IAsyncDocumentSession _documentSession;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="documentSession">The document session.</param>
        public UserService(IAsyncDocumentSession documentSession)
        {
            this._documentSession = documentSession;
        }
        #endregion

        #region Implementation of IUserService
        /// <summary>
        /// Returns the current user.
        /// </summary>
        public async Task<User> GetCurrentUser()
        {
            string userId = Thread.CurrentPrincipal.Identity.Name;

            if (string.IsNullOrWhiteSpace(userId))
                throw new InvalidOperationException("You can only access the current user when the controller-method has the 'RequiresAuthentication' attribute.");

            return await this._documentSession.LoadAsync<User>(userId);
        }
        #endregion
    }
}
