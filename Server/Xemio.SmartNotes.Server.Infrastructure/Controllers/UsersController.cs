using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Raven.Abstractions.Data;
using Raven.Client;
using Xemio.SmartNotes.Models.Entities.Users;
using Xemio.SmartNotes.Models.Models;
using Xemio.SmartNotes.Server.Abstractions.Services;
using Xemio.SmartNotes.Server.Infrastructure.Exceptions;
using Xemio.SmartNotes.Server.Infrastructure.Extensions;
using Xemio.SmartNotes.Server.Infrastructure.Filters;
using Xemio.SmartNotes.Server.Infrastructure.Properties;
using Xemio.SmartNotes.Server.Infrastructure.Raven.Indexes;

namespace Xemio.SmartNotes.Server.Infrastructure.Controllers
{
    /// <summary>
    /// Controller for the <see cref="User"/> class.
    /// </summary>
    public class UsersController : BaseController
    {
        #region Fields
        private readonly IEmailValidationService _emailValidationService;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
        /// </summary>
        /// <param name="documentSession">The document session.</param>
        /// <param name="emailValidationService">The email validator.</param>
        /// <param name="userService">The user service.</param>
        public UsersController(IDocumentSession documentSession, IEmailValidationService emailValidationService, IUserService userService)
            : base(documentSession, userService)
        {
            this._emailValidationService = emailValidationService;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Creates a new <see cref="User"/>.
        /// </summary>
        /// <param name="user">The new user.</param>
        [Route("Users")]
        public HttpResponseMessage PostUser([FromBody]User user)
        {
            if (user == null)
                throw new InvalidRequestException();

            if (string.IsNullOrWhiteSpace(user.Username))
                throw new InvalidUsernameException();

            if (this.IsUsernameAvailable(user.Username) == false)
                throw new UsernameUnavailableException(user.Username);

            if (this._emailValidationService.IsValidEmailAddress(user.EmailAddress) == false)
                throw new InvalidEmailAddressException(user.EmailAddress);

            if (this.IsEmailAddressAvailable(user.EmailAddress) == false)
                throw new EmailAddressUnavailableException(user.EmailAddress);

            this.DocumentSession.Store(user);

            this.Logger.DebugFormat("Created new user '{0}' with username '{1}'.", user.Id, user.Username);

            return Request.CreateResponse(HttpStatusCode.Created, user);
        }
        /// <summary>
        /// Gets the current user.
        /// </summary>
        [Route("Users/Authorized")]
        [RequiresAuthorization]
        public HttpResponseMessage GetAuthorized()
        {
            return Request.CreateResponse(HttpStatusCode.Found, this.UserService.GetCurrentUser());
        }
        /// <summary>
        /// Updates the <see cref="User"/>.
        /// </summary>
        /// <param name="user">The user.</param>
        [Route("Users/Authorized")]
        [RequiresAuthorization]
        public HttpResponseMessage PutUser([FromBody]User user)
        {
            if (this._emailValidationService.IsValidEmailAddress(user.EmailAddress) == false)
                throw new InvalidEmailAddressException(user.EmailAddress);

            User currentUser = this.UserService.GetCurrentUser();

            if (this.GetUserWithEmailAddress(user.EmailAddress) != currentUser)
                throw new EmailAddressUnavailableException(user.EmailAddress);

            currentUser.EmailAddress = user.EmailAddress;
            currentUser.AuthorizationHash = user.AuthorizationHash;

            this.Logger.DebugFormat("Updated user '{0}'.", currentUser.Id);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Determines whether the given <paramref name="username"/> is available.
        /// </summary>
        private bool IsUsernameAvailable(string username)
        {
            return this.DocumentSession.Query<User>().Any(f => f.Username == username) == false;
        }
        /// <summary>
        /// Determines whether the given <paramref name="emailAddress"/> is available.
        /// </summary>
        /// <param name="emailAddress">The email address.</param>
        private bool IsEmailAddressAvailable(string emailAddress)
        {
            return this.DocumentSession.Query<User>().Any(f => f.EmailAddress == emailAddress) == false;
        }
        /// <summary>
        /// Gets the user with email address.
        /// </summary>
        /// <param name="emailAddress">The email address.</param>
        private User GetUserWithEmailAddress(string emailAddress)
        {
            return this.DocumentSession.Query<User>().FirstOrDefault(f => f.EmailAddress == emailAddress);
        }
        #endregion
    }
}
