using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Raven.Abstractions.Data;
using Raven.Client;
using Xemio.SmartNotes.Server.Abstractions.Authentication;
using Xemio.SmartNotes.Server.Abstractions.Services;
using Xemio.SmartNotes.Server.Infrastructure.Exceptions;
using Xemio.SmartNotes.Server.Infrastructure.Extensions;
using Xemio.SmartNotes.Server.Infrastructure.Filters;
using Xemio.SmartNotes.Server.Infrastructure.Properties;
using Xemio.SmartNotes.Server.Infrastructure.RavenDB.Indexes;
using Xemio.SmartNotes.Shared.Entities.Users;
using Xemio.SmartNotes.Shared.Models;

namespace Xemio.SmartNotes.Server.Infrastructure.Controllers
{
    /// <summary>
    /// Controller for the <see cref="User"/> class.
    /// </summary>
    public class UsersController : BaseController
    {
        #region Fields
        private readonly IEmailValidationService _emailValidationService;
        private readonly IExampleDataService _exampleDataService;
        private readonly IAuthenticationProvider[] _authenticationProviders;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
        /// </summary>
        /// <param name="documentSession">The document session.</param>
        /// <param name="emailValidationService">The email validator.</param>
        /// <param name="userService">The createUser service.</param>
        /// <param name="exampleDataService">The example data service.</param>
        /// <param name="authenticationProviders">The authentication providers.</param>
        public UsersController(IDocumentSession documentSession, IEmailValidationService emailValidationService, IUserService userService, IExampleDataService exampleDataService, IAuthenticationProvider[] authenticationProviders)
            : base(documentSession, userService)
        {
            this._emailValidationService = emailValidationService;
            this._exampleDataService = exampleDataService;
            this._authenticationProviders = authenticationProviders;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Creates a new <see cref="User"/>.
        /// </summary>
        /// <param name="createUser">The new createUser.</param>
        [Route("Users")]
        public HttpResponseMessage PostUser([FromBody]CreateUser createUser)
        {
            if (createUser == null)
                throw new InvalidRequestException();

            var user = new User
            {
                EmailAddress = createUser.EmailAddress,
                PreferredLanguage = createUser.PreferredLanguage,
                TimeZoneId = createUser.TimeZoneId
            };

            this.DocumentSession.Store(user);

            IAuthenticationProvider authenticationProvider = this._authenticationProviders.FirstOrDefault(f => f.Type == createUser.AuthenticationType);

            if (authenticationProvider == null)
                throw new ApplicationException(string.Format("No authentication provider for the authentication type '{0}' was found.", createUser.AuthenticationType));

            if (authenticationProvider.Register(user, createUser.AuthenticationData) == false)
                throw new RegistrationFailedException();

            //We validate the email-address after we register the user with the authentication provider
            //So the authentication provider (like facebook) can set the email address
            //No problem because ravendb is transactional
            if (this._emailValidationService.IsValidEmailAddress(user.EmailAddress) == false)
                throw new InvalidEmailAddressException(user.EmailAddress);

            if (this.IsEmailAddressAvailable(user.EmailAddress) == false)
                throw new EmailAddressUnavailableException(user.EmailAddress);

            this.Logger.DebugFormat("Created new createUser '{0}' with email address '{1}'.", user.Id, user.EmailAddress);

            return Request.CreateResponse(HttpStatusCode.Created, user);
        }
        /// <summary>
        /// Gets the current createUser.
        /// </summary>
        [Route("Users/Me")]
        [RequiresAuthorization]
        public HttpResponseMessage GetMe()
        {
            this._exampleDataService.CreateExampleDataForCurrentUser();

            return Request.CreateResponse(HttpStatusCode.Found, this.UserService.GetCurrentUser());
        }
        /// <summary>
        /// Updates the <see cref="User"/>.
        /// </summary>
        /// <param name="user">The createUser.</param>
        [Route("Users/Me")]
        [RequiresAuthorization]
        public HttpResponseMessage PutUser([FromBody]User user)
        {
            if (this._emailValidationService.IsValidEmailAddress(user.EmailAddress) == false)
                throw new InvalidEmailAddressException(user.EmailAddress);

            User currentUser = this.UserService.GetCurrentUser();

            if (this.GetUserWithEmailAddress(user.EmailAddress) != currentUser)
                throw new EmailAddressUnavailableException(user.EmailAddress);

            currentUser.EmailAddress = user.EmailAddress;

            this.Logger.DebugFormat("Updated createUser '{0}'.", currentUser.Id);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Determines whether the given <paramref name="emailAddress"/> is available.
        /// </summary>
        /// <param name="emailAddress">The email address.</param>
        private bool IsEmailAddressAvailable(string emailAddress)
        {
            return this.DocumentSession.Query<User, UsersByEmailAddress>()
                .Any(f => f.EmailAddress == emailAddress) == false;
        }
        /// <summary>
        /// Gets the createUser with email address.
        /// </summary>
        /// <param name="emailAddress">The email address.</param>
        private User GetUserWithEmailAddress(string emailAddress)
        {
            return this.DocumentSession.Query<User, UsersByEmailAddress>()
                .FirstOrDefault(f => f.EmailAddress == emailAddress);
        }
        #endregion
    }
}
