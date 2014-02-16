using System;

namespace Xemio.SmartNotes.Shared.Entities.Users
{
    /// <summary>
    /// Contains information about a requested password reset.
    /// </summary>
    public class PasswordReset
    {
        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// Gets or sets the secret used for resetting the password.
        /// </summary>
        public string Secret { get; set; }
        /// <summary>
        /// Gets or sets the <see cref="DateTimeOffset"/> when the reset was requested.
        /// </summary>
        public DateTimeOffset RequestedAt { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether the password has been reset.
        /// </summary>
        public bool PasswordWasReset { get; set; }
    }
}
