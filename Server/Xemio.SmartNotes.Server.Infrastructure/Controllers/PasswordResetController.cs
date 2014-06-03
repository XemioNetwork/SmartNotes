using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using Raven.Abstractions.Data;
using Raven.Client;
using Xemio.SmartNotes.Server.Abstractions.Authentication;
using Xemio.SmartNotes.Server.Abstractions.Mailing;
using Xemio.SmartNotes.Server.Abstractions.Security;
using Xemio.SmartNotes.Server.Abstractions.Services;
using Xemio.SmartNotes.Server.Infrastructure.Exceptions;
using Xemio.SmartNotes.Server.Infrastructure.Extensions;
using Xemio.SmartNotes.Shared.Entities.Users;
using Xemio.SmartNotes.Shared.Models;

namespace Xemio.SmartNotes.Server.Infrastructure.Controllers
{
    public class PasswordResetController : BaseController
    {
        #region Constants
        private static readonly TimeSpan TimeToResetPassword = TimeSpan.FromHours(2);
        #endregion

        #region Fields
        private readonly ISecretGenerator _secretGenerator;
        private readonly IEmailFactory _emailFactory;
        private readonly IAuthenticationProvider[] _authenticationProviders;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="PasswordResetController"/> class.
        /// </summary>
        /// <param name="documentSession">The document session.</param>
        /// <param name="userService">The user service.</param>
        /// <param name="secretGenerator">The secret generator.</param>
        /// <param name="emailFactory">The email factory.</param>
        /// <param name="authenticationProviders">The authentication providers.</param>
        public PasswordResetController(IDocumentSession documentSession, IUserService userService, ISecretGenerator secretGenerator, IEmailFactory emailFactory, IAuthenticationProvider[] authenticationProviders)
            : base(documentSession, userService)
        {
            this._secretGenerator = secretGenerator;
            this._emailFactory = emailFactory;
            this._authenticationProviders = authenticationProviders;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Creates a new <see cref="PasswordReset"/>.
        /// </summary>
        /// <param name="data">The username or the email address of the user.</param>
        [Route("PasswordResets")]
        public HttpResponseMessage PostPasswordReset(CreatePasswordReset data)
        {
            if (data == null)
                throw new InvalidRequestException();

            User user = this.GetUser(data.UsernameOrEmailAddress);
            
            //We have to set the language here because it's a request without authorization.
            //Because of that we haven't set the language yet and do it now.
            this.SetLanguage(user);

            var passwordReset = new PasswordReset
                                {
                                    UserId = user.Id,
                                    Secret = this._secretGenerator.GenerateString(32),
                                    RequestedAt = DateTimeOffset.Now
                                };

            this.DocumentSession.Store(passwordReset);

            string uri = this.GetBaseUri() + "/PasswordResets?secret=" + passwordReset.Secret;
            this._emailFactory.SendPasswordForgotEmail(user, uri);

            this.Logger.DebugFormat("User '{0}' requested a password reset.", user.Id);

            return Request.CreateResponse(HttpStatusCode.Created);
        }
        /// <summary>
        /// Finishes a password reset.
        /// </summary>
        /// <param name="secret">The secret.</param>
        [Route("PasswordResets")]
        public HttpResponseMessage GetPasswordReset(string secret)
        {
            if (string.IsNullOrWhiteSpace(secret))
                throw new InvalidPasswordResetSecretException();

            var passwordReset =  this.DocumentSession
                .Query<PasswordReset>()
                .Customize(f => f.WaitForNonStaleResultsAsOfLastWrite())
                .Include(f => f.UserId)
                .FirstOrDefault(f => f.Secret == secret);

            if (passwordReset == null)
                throw new InvalidPasswordResetSecretException();

            if (passwordReset.PasswordWasReset)
                throw new PasswordAlreadyResetException();

            if (DateTimeOffset.UtcNow > passwordReset.RequestedAt.Add(TimeToResetPassword))
                throw new PasswordResetTimedOutException();

            var user = this.DocumentSession.Load<User>(passwordReset.UserId);

            //We have to set the language here because it's a request without authorization.
            //Because of that we haven't set the language yet and do it now.
            this.SetLanguage(user);

            string newPassword = this._secretGenerator.GenerateString(16);

            IAuthenticationProvider authenticationProvider = this._authenticationProviders.First(f => f.Type == AuthenticationType.Xemio);
            authenticationProvider.Update(user, new JObject
            {
                {"Password", newPassword}
            });

            this._emailFactory.SendPasswordResetEmail(user, newPassword);

            passwordReset.PasswordWasReset = true;

            this.Logger.DebugFormat("Reset the password of user '{0}'.", user.Id);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Gets the user.
        /// </summary>
        /// <param name="usernameOrEmailAddress">The username or email address.</param>
        private User GetUser(string usernameOrEmailAddress)
        {
            XemioAuthentication authentication = this.GetLocalAuthentication(usernameOrEmailAddress);

            if (authentication != null)
            {
                return this.DocumentSession.Load<User>(authentication.UserId);
            }
            
            throw new InvalidRequestException();
        }
        /// <summary>
        /// Gets the local authentication.
        /// </summary>
        /// <param name="usernameOrEmailAddress">The username or email address.</param>
        private XemioAuthentication GetLocalAuthentication(string usernameOrEmailAddress)
        {
            User user = this.DocumentSession.Query<User>().FirstOrDefault(f => f.EmailAddress == usernameOrEmailAddress);

            if (user != null)
            {
                return this.DocumentSession.Query<XemioAuthentication>()
                    .Customize(f => f.WaitForNonStaleResultsAsOfLastWrite())
                    .FirstOrDefault(f => f.UserId == user.Id);
            }

            return null;
        }
        #endregion
    }
}
