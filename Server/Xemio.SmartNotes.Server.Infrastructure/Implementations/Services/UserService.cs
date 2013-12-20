using System;
using System.Threading;
using System.Threading.Tasks;
using Raven.Client;
using Xemio.SmartNotes.Models.Entities.Users;
using Xemio.SmartNotes.Server.Abstractions.Services;

namespace Xemio.SmartNotes.Server.Infrastructure.Implementations.Services
{
    public class UserService : IUserService
    {
        #region Fields
        private readonly IDocumentSession _documentSession;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="documentSession">The document session.</param>
        public UserService(IDocumentSession documentSession)
        {
            this._documentSession = documentSession;
        }
        #endregion

        #region Implementation of IUserService
        /// <summary>
        /// Returns the current user.
        /// </summary>
        public User GetCurrentUser()
        {
            string userId = Thread.CurrentPrincipal.Identity.Name;

            if (string.IsNullOrWhiteSpace(userId))
                throw new InvalidOperationException("You can only access the current user when the controller-method has the 'RequiresAuthentication' attribute.");

            return this._documentSession.Load<User>(userId);
        }
        #endregion
    }
}
