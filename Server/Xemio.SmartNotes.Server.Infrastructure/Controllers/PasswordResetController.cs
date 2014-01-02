using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Raven.Abstractions.Data;
using Raven.Client;
using Xemio.SmartNotes.Abstractions.Authorization;
using Xemio.SmartNotes.Models.Entities.Users;
using Xemio.SmartNotes.Models.Models;
using Xemio.SmartNotes.Server.Abstractions.Email;
using Xemio.SmartNotes.Server.Abstractions.Security;
using Xemio.SmartNotes.Server.Infrastructure.Exceptions;

namespace Xemio.SmartNotes.Server.Infrastructure.Controllers
{
    public class PasswordResetController : BaseController
    {
        #region Constants
        private static readonly TimeSpan TimeToResetPassword = TimeSpan.FromHours(2);
        #endregion

        #region Fields
        private readonly ISecretGenerator _secretGenerator;
        private readonly IEmailSender _emailSender;
        private readonly IEmailFactory _emailFactory;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="PasswordResetController"/> class.
        /// </summary>
        /// <param name="documentSession">The document session.</param>
        /// <param name="secretGenerator">The secret generator.</param>
        /// <param name="emailSender">The email sender.</param>
        /// <param name="emailFactory">The email factory.</param>
        public PasswordResetController(IDocumentSession documentSession, ISecretGenerator secretGenerator, IEmailSender emailSender, IEmailFactory emailFactory)
            : base(documentSession)
        {
            this._secretGenerator = secretGenerator;
            this._emailSender = emailSender;
            this._emailFactory = emailFactory;
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

            var passwordReset = new PasswordReset
                                {
                                    UserId = user.Id,
                                    Secret = this._secretGenerator.Generate(),
                                    RequestedAt = DateTimeOffset.Now
                                };

            this.DocumentSession.Store(passwordReset);

            string uri = this.GetBaseUri() + "/PasswordResets?secret=" + passwordReset.Secret;
            this._emailSender.SendAsync(this._emailFactory.CreatePasswordForgotEmail(user, uri));

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
                .Include<PasswordReset>(f => f.UserId)
                .FirstOrDefault(f => f.Secret == secret);

            if (passwordReset == null)
                throw new InvalidPasswordResetSecretException();

            if (passwordReset.PasswordWasReset)
                throw new PasswordAlreadyResetException();

            if (DateTimeOffset.UtcNow > passwordReset.RequestedAt.Add(TimeToResetPassword))
                throw new PasswordResetTimedOutException();

            var user = this.DocumentSession.Load<User>(passwordReset.UserId);

            string newPassword = this._secretGenerator.Generate(16);
            user.AuthorizationHash = AuthorizationHash.CreateBaseHash(user.Username, newPassword);

            this._emailSender.SendAsync(this._emailFactory.CreatePasswordResetEmail(user, newPassword));

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
            User user = this.DocumentSession.Query<User>().FirstOrDefault(f => f.EmailAddress == usernameOrEmailAddress) ??
                        this.DocumentSession.Query<User>().FirstOrDefault(f => f.Username == usernameOrEmailAddress);

            if (user != null)
                return user;

            throw new InvalidRequestException();
        }
        #endregion
    }
}
