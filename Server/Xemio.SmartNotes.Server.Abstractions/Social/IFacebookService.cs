using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xemio.SmartNotes.Server.Abstractions.Social
{
    public interface IFacebookService
    {
        /// <summary>
        /// Exchanges the specified <paramref name="token"/> for an access token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="redirectUri">The redirect URI.</param>
        string ExchangeTokenForAccessToken(string token, string redirectUri);

        /// <summary>
        /// Gets the facebook user.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        FacebookUser GetFacebookUser(string accessToken);
    }
}
