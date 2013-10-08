using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using AttributeRouting.Web.Http;
using Raven.Client;
using Xemio.SmartNotes.Abstractions.Controllers;
using Xemio.SmartNotes.Core.Exceptions;
using Xemio.SmartNotes.Core.Services;
using Xemio.SmartNotes.Entities.Users;
using Xemio.SmartNotes.Infrastructure.Authentication;
using Xemio.SmartNotes.Infrastructure.Extensions;
using Xemio.SmartNotes.Infrastructure.Raven.Indexes;
using Xemio.SmartNotes.Models.Users;
using System.Security.Cryptography;

namespace Xemio.SmartNotes.Infrastructure.Controllers
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
        /// Gets the current user.
        /// </summary>
        [GET("Users/Current")]
        [RequiresAuthentication]
        public async Task<HttpResponseMessage> GetCurrent()
        {
            return Request.CreateResponse(HttpStatusCode.Found, await this._userService.GetCurrentUser());
        }
        /// <summary>
        /// Updates the <see cref="User"/>.
        /// </summary>
        /// <param name="user">The user.</param>
        [PUT("Users/Current")]
        [RequiresAuthentication]
        public async Task<HttpResponseMessage> PutUser([FromBody]User user)
        {
            if (this._emailValidationService.IsValidEmailAddress(user.EMailAddress) == false)
                throw new InvalidEmailAddressException(user.EMailAddress);

            User currentUser = await this._userService.GetCurrentUser();

            currentUser.EMailAddress = user.EMailAddress;

            return Request.CreateResponse(HttpStatusCode.OK);
        }
        /// <summary>
        /// Creates a new <see cref="User"/>.
        /// </summary>
        /// <param name="createUser">The createUser.</param>
        [POST("Users")]
        public async Task<HttpResponseMessage> PostUser([FromBody]PostUser createUser)
        {
            if (createUser == null)
                throw new InvalidRequestException();

            if (string.IsNullOrWhiteSpace(createUser.Username))
                throw new InvalidUsernameException();

            if (await this.IsUsernameAvailable(createUser.Username) == false)
                throw new UsernameUnavailableException(createUser.Username);

            if (string.IsNullOrWhiteSpace(createUser.Password) || createUser.Password.Length < 7)
                throw new InvalidPasswordException();

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
                AuthenticationHash = this.ComputeAuthenticationHash(createUser.Username, createUser.Password)
            };
            await this.DocumentSession.StoreAsync(authenticationData);

            return Request.CreateResponse(HttpStatusCode.Created, user);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Computes the authentication hash.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        private byte[] ComputeAuthenticationHash(string username, string password)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(username + password);
            return SHA256.Create().ComputeHash(bytes);
        }
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
