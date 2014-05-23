using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using Raven.Client;
using Xemio.SmartNotes.Server.Abstractions.Authentication;
using Xemio.SmartNotes.Server.Abstractions.Security;
using Xemio.SmartNotes.Server.Abstractions.Services;
using Xemio.SmartNotes.Server.Infrastructure.Exceptions;
using Xemio.SmartNotes.Server.Infrastructure.Extensions;
using Xemio.SmartNotes.Shared.Entities.Users;
using Xemio.SmartNotes.Shared.Models;

namespace Xemio.SmartNotes.Server.Infrastructure.Controllers
{
    public class TokenController : BaseController
    {
        #region Fields
        private readonly IAuthenticationProvider[] _authenticationProviders;
        private readonly ISecretGenerator _secretGenerator;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="TokenController"/> class.
        /// </summary>
        /// <param name="authenticationProviders">The authentication providers.</param>
        /// <param name="secretGenerator">The secret generator.</param>
        /// <param name="documentSession">The document session.</param>
        /// <param name="userService">The user service.</param>
        public TokenController(IAuthenticationProvider[] authenticationProviders, ISecretGenerator secretGenerator, IDocumentSession documentSession, IUserService userService) 
            : base(documentSession, userService)
        {
            this._authenticationProviders = authenticationProviders;
            this._secretGenerator = secretGenerator;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Creates a new token.
        /// </summary>
        /// <param name="createToken">The create token data.</param>
        [Route("Token")]
        public HttpResponseMessage PostToken([FromBody]CreateToken createToken)
        {
            IAuthenticationProvider provider = this._authenticationProviders.SingleOrDefault(f => f.Type == createToken.Type);
            
            if (provider == null)
                throw new ApplicationException(string.Format("No authentication provider for the authentication type '{0}' was found.", createToken.Type));

            AuthenticationResult result = provider.Authenticate(createToken.AuthenticationData);

            if (result.Successfull == false)
                throw new AuthenticationFailedException(result.AdditionalData);

            var now = DateTimeOffset.UtcNow;

            var token = new AuthenticationToken
            {
                Token = this._secretGenerator.GenerateString(),
                CreatedDate = now,
                ValidUntil = now.AddDays(2),
                UserId = result.UserId,
                AuthenticationType = createToken.Type
            };

            this.DocumentSession.Store(token);

            return Request.CreateResponse(HttpStatusCode.OK, token);
        }
        #endregion
    }
}
