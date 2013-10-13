﻿using System.Runtime.Serialization;

namespace Xemio.SmartNotes.Entities.Users
{
    /// <summary>
    /// Contains information about a single <see cref="User" />.
    /// </summary>
    public class User : AggregateRoot
    {
        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        public string EMailAddress { get; set; }
        /// <summary>
        /// Gets or sets the avatar image.
        /// </summary>
        public byte[] Avatar { get; set; }
    }
}
