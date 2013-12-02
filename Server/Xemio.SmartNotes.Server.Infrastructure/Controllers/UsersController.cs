using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Raven.Abstractions.Data;
using Raven.Client;
using Xemio.SmartNotes.Abstractions.Controllers;
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
    public class UsersController : BaseController, IUsersController
    {
        #region Fields
        private readonly IEmailValidationService _emailValidationService;
        private readonly IUserService _userService;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
        /// </summary>
        /// <param name="documentSession">The document session.</param>
        /// <param name="emailValidationService">The email validator.</param>
        /// <param name="userService">The user service.</param>
        public UsersController(IAsyncDocumentSession documentSession, IEmailValidationService emailValidationService, IUserService userService)
            : base(documentSession)
        {
            this._emailValidationService = emailValidationService;
            this._userService = userService;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Creates a new <see cref="User"/>.
        /// </summary>
        /// <param name="createUser">The createUser.</param>
        [Route("Users")]
        public async Task<HttpResponseMessage> PostUser([FromBody]CreateUser createUser)
        {
            if (createUser == null)
                throw new InvalidRequestException();

            if (string.IsNullOrWhiteSpace(createUser.Username))
                throw new InvalidUsernameException();

            if (await this.IsUsernameAvailable(createUser.Username) == false)
                throw new UsernameUnavailableException(createUser.Username);

            if (this._emailValidationService.IsValidEmailAddress(createUser.EmailAddress) == false)
                throw new InvalidEmailAddressException(createUser.EmailAddress);

            if (await this.IsEmailAddressAvailable(createUser.EmailAddress) == false)
                throw new EmailAddressUnavailableException(createUser.EmailAddress);

            var user = new User
            {
                Username = createUser.Username,
                EmailAddress = createUser.EmailAddress,
            };
            await this.DocumentSession.StoreAsync(user);

            var authenticationData = new UserAuthentication
            {
                UserId = user.Id,
                AuthorizationHash = createUser.AuthorizationHash
            };
            await this.DocumentSession.StoreAsync(authenticationData);

            this.Logger.DebugFormat("Created new user '{0}' with username '{1}'.", user.Id, user.Username);

            return Request.CreateResponse(HttpStatusCode.Created, user);
        }
        /// <summary>
        /// Gets the current user.
        /// </summary>
        [Route("Users/Authorized")]
        [RequiresAuthorization]
        public async Task<HttpResponseMessage> GetAuthorized()
        {
            return Request.CreateResponse(HttpStatusCode.Found, await this._userService.GetCurrentUser());
        }
        /// <summary>
        /// Updates the <see cref="User"/>.
        /// </summary>
        /// <param name="user">The user.</param>
        [Route("Users/Authorized")]
        [RequiresAuthorization]
        public async Task<HttpResponseMessage> PutUser([FromBody]User user)
        {
            if (this._emailValidationService.IsValidEmailAddress(user.EmailAddress) == false)
                throw new InvalidEmailAddressException(user.EmailAddress);

            User currentUser = await this._userService.GetCurrentUser();

            currentUser.EmailAddress = user.EmailAddress;

            this.Logger.DebugFormat("Updated user '{0}'.", currentUser.Id);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Determines whether the given <paramref name="username"/> is available.
        /// </summary>
        private async Task<bool> IsUsernameAvailable(string username)
        {
            return (await this.DocumentSession.Query<User>().AnyAsync(f => f.Username == username)) == false;
        }
        /// <summary>
        /// Determines whether the given <paramref name="emailAddress"/> is available.
        /// </summary>
        /// <param name="emailAddress">The email address.</param>
        private async Task<bool> IsEmailAddressAvailable(string emailAddress)
        {
            return (await this.DocumentSession.Query<User>().AnyAsync(f => f.EmailAddress == emailAddress)) == false;
        }
        #endregion
    }
}
