using System;
using Xemio.SmartNotes.Shared.Entities.Users;
using Xemio.SmartNotes.Shared.Extensions;

namespace Xemio.SmartNotes.Client.Shared.Clients
{
    /// <summary>
    /// The data of the current application.
    /// </summary>
    public class Session
    {
        /// <summary>
        /// Gets or sets the token.
        /// </summary>
        public AuthenticationToken Token { get; set; }

        /// <summary>
        /// Gets or sets the current user.
        /// </summary>
        public User User { get; set; }
    }
}
