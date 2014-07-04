using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CuttingEdge.Conditions;
using Newtonsoft.Json.Linq;
using Raven.Client;
using Xemio.SmartNotes.Server.Abstractions.Authentication;
using Xemio.SmartNotes.Server.Abstractions.Security;
using Xemio.SmartNotes.Server.Infrastructure.Exceptions;
using Xemio.SmartNotes.Server.Infrastructure.Extensions;
using Xemio.SmartNotes.Server.Infrastructure.RavenDB.Indexes;
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
            Condition.Requires(data, "data")
                .IsNotNull();

            var authenticationData = data.ToObject<AuthenticationData>();

            var user = this._documentSession.Query<User, UsersByEmailAddress>()
                .FirstOrDefault(f => f.EmailAddress == authenticationData.EmailAddress);

            if (user == null)
                return AuthenticationResult.Failure();

            string id = this._documentSession.Advanced.GetStringIdFor<XemioAuthentication>(user.Id);
            var authentication = this._documentSession.Load<XemioAuthentication>(id);

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
            Condition.Requires(user, "user")
                .IsNotNull();
            Condition.Requires(data, "data")
                .IsNotNull();

            var registerData = data.ToObject<RegisterData>();

            byte[] salt = this._secretGenerator.Generate();

            var authentication = new XemioAuthentication
            {
                UserId = user.Id,
                Salt = salt,
                PasswordHash = this._saltCombiner.Combine(salt, registerData.Password),
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
            Condition.Requires(user, "user")
                .IsNotNull();
            Condition.Requires(data, "data")
                .IsNotNull();

            var updateData = data.ToObject<UpdateData>();

            string id = this._documentSession.Advanced.GetStringIdFor<XemioAuthentication>(user.Id);
            var authentication = this._documentSession.Load<XemioAuthentication>(id);

            byte[] salt = this._secretGenerator.Generate();
            authentication.Salt = salt;
            authentication.PasswordHash = this._saltCombiner.Combine(salt, updateData.Password);
        }
        #endregion

        #region Private
        private class AuthenticationData
        {
            public string EmailAddress { get; set; }
            public string Password { get; set; }
        }

        private class RegisterData
        {
            public string Password { get; set; }
        }

        private class UpdateData
        {
            public string Password { get; set; }
        }
        #endregion
    }
}
