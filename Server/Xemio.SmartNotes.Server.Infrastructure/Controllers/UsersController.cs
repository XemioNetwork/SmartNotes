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

            if (this._emailValidationService.IsValidEmailAddress(createUser.EMailAddress) == false)
                throw new InvalidEmailAddressException(createUser.EMailAddress);

            var user = new User
            {
                Username = createUser.Username,
                EMailAddress = createUser.EMailAddress,
            };
            await this.DocumentSession.StoreAsync(user);

            var authenticationData = new UserAuthentication
            {
                UserId = user.Id,
                AuthorizationHash = createUser.AuthorizationHash
            };
            await this.DocumentSession.StoreAsync(authenticationData);

            return Request.CreateResponse(HttpStatusCode.Created, user);
        }
        /// <summary>
        /// Gets the current user.
        /// </summary>
        [Route("Users/Authorized")]
        [RequiresAuthorization]
        public async Task<HttpResponseMessage> GetCurrent()
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
            if (this._emailValidationService.IsValidEmailAddress(user.EMailAddress) == false)
                throw new InvalidEmailAddressException(user.EMailAddress);

            User currentUser = await this._userService.GetCurrentUser();

            currentUser.EMailAddress = user.EMailAddress;

            return Request.CreateResponse(HttpStatusCode.OK);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Determines whether the given <paramref name="username"/> is available.
        /// </summary>
        private async Task<bool> IsUsernameAvailable(string username)
        {
            return (await this.DocumentSession.Query<User, UsersByUsername>().AnyAsync(f => f.Username == username)) == false;
        }
        #endregion
    }
}
