using System.Linq;
using Castle.Windsor.Installer;
using Raven.Client.Indexes;
using Xemio.SmartNotes.Shared.Entities.Users;

namespace Xemio.SmartNotes.Server.Infrastructure.RavenDB.Indexes
{
    public class AuthenticationTokensByToken : AbstractIndexCreationTask<AuthenticationToken>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationTokensByToken"/> class.
        /// </summary>
        public AuthenticationTokensByToken()
        {
            this.Map = authTokens => from authToken in authTokens
                                     select new
                                     {
                                         authToken.Token
                                     };
        }
        /// <summary>
        /// Gets the name of the index.
        /// </summary>
        public override string IndexName
        {
            get { return "AuthenticationTokens/ByToken"; }
        }
    }
}