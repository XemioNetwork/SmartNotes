﻿using System;
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
        /// Gets or sets the username.
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the current user.
        /// </summary>
        public User User { get; set; }

        #region Methods
        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        public int GetUserId()
        {
            if (this.User == null)
                throw new InvalidOperationException("The 'User' is null. You need to set the 'User' property to user the 'GetUserId' method.");

            return this.User.Id.GetIntId();
        }
        #endregion
    }
}