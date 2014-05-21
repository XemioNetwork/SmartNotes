using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Raven.Client;
using Xemio.SmartNotes.Server.Abstractions.Authentication;
using Xemio.SmartNotes.Server.Abstractions.Security;
using Xemio.SmartNotes.Server.Infrastructure.Exceptions;
using Xemio.SmartNotes.Shared.Entities.Users;

namespace Xemio.SmartNotes.Server.Infrastructure.Implementations.Authentication
{
    public class XemioAuthenticationProvider : IAuthenticationProvider
    {
        #region Fields
        private readonly IDocumentSession _documentSession;
        private readonly ISaltCombiner _saltCombiner;
        private readonly ISecretGenerator _secretGenerator;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="XemioAuthenticationProvider"/> class.
        /// </summary>
        /// <param name="documentSession">The document session.</param>
        /// <param name="saltCombiner">The salt combiner.</param>
        /// <param name="secretGenerator">The secret generator.</param>
        public XemioAuthenticationProvider(IDocumentSession documentSession, ISaltCombiner saltCombiner, ISecretGenerator secretGenerator)
        {
            this._documentSession = documentSession;
            this._saltCombiner = saltCombiner;
            this._secretGenerator = secretGenerator;
        }
        #endregion

        #region Implementation of IAuthenticationProvider
        /// <summary>
        /// Gets the authentication type.
        /// </summary>
        public AuthenticationType Type
        {
            get {  return AuthenticationType.Xemio; }
        }
        /// <summary>
        /// Tries to authenticate with the specified <paramref name="data" />.
        /// </summary>
        /// <param name="data">The data.</param>
        public AuthenticationResult Authenticate(JObject data)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            var authenticationData = data.ToObject<AuthenticationData>();

            var authentication = this._documentSession.Query<XemioAuthentication>()
                .Customize(f => f.WaitForNonStaleResultsAsOfLastWrite())
                .FirstOrDefault(f => f.Username == authenticationData.Username);

            if (authentication == null)
                return AuthenticationResult.Failure();

            byte[] saltWithPassword = this._saltCombiner.Combine(authentication.Salt, authenticationData.Password);

            if (saltWithPassword.SequenceEqual(authentication.PasswordHash))
            {
                return AuthenticationResult.Success(authentication.UserId);
            }
            else
            {
                return AuthenticationResult.Failure();
            }
        }
        /// <summary>
        /// Registers the specified <paramref name="user" /> with the specified <paramref name="data" />.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="data">The data.</param>
        public bool Register(User user, JObject data)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            if (data == null)
                throw new ArgumentNullException("data");

            var registerData = data.ToObject<RegisterData>();

            if (string.IsNullOrWhiteSpace(registerData.Username))
                throw new InvalidUsernameException();

            if (this.IsUsernameAvailable(registerData.Username) == false)
                throw new UsernameUnavailableException(registerData.Username);

            byte[] salt = this._secretGenerator.Generate();

            var authentication = new XemioAuthentication
            {
                UserId = user.Id,
                Salt = salt,
                PasswordHash = this._saltCombiner.Combine(salt, registerData.Password),
                Username = registerData.Username
            };

            this._documentSession.Store(authentication);

            return true;
        }
        /// <summary>
        /// Updates the authentication of the specified <paramref name="user" />.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="data">The data.</param>
        public void Update(User user, JObject data)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            if (data == null)
                throw new ArgumentNullException("data");

            var updateData = data.ToObject<UpdateData>();

            var authentication = this._documentSession.Query<XemioAuthentication>()
                .Customize(f => f.WaitForNonStaleResultsAsOfLastWrite())
                .First(f => f.UserId == user.Id);

            byte[] salt = this._secretGenerator.Generate();
            authentication.Salt = salt;
            authentication.PasswordHash = this._saltCombiner.Combine(salt, updateData.Password);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Determines whether the given <paramref name="username"/> is available.
        /// </summary>
        private bool IsUsernameAvailable(string username)
        {
            return this._documentSession.Query<XemioAuthentication>()
                .Customize(f => f.WaitForNonStaleResultsAsOfLastWrite())
                .Any(f => f.Username == username) == false;
        }
        #endregion

        #region Private
        private class AuthenticationData
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        private class RegisterData
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        private class UpdateData
        {
            public string Password { get; set; }
        }
        #endregion
    }
}
