using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Xemio.SmartNotes.Shared.Entities.Users;

namespace Xemio.SmartNotes.Server.Abstractions.Authentication
{
    public interface IAuthenticationProvider
    {
        /// <summary>
        /// Gets the type of the authentication.
        /// </summary>
        AuthenticationType Type { get; }

        /// <summary>
        /// Tries to authenticate with the specified <paramref name="data"/>.
        /// </summary>
        /// <param name="data">The data.</param>
        AuthenticationResult Authenticate(JObject data);

        /// <summary>
        /// Registers the specified <paramref name="user"/> with the specified <paramref name="data"/>.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="data">The data.</param>
        bool Register(User user, JObject data);

        /// <summary>
        /// Updates the authentication of the specified <paramref name="user"/>.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="data">The data.</param>
        void Update(User user, JObject data);
    }
}
