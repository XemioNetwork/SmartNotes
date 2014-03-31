using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xemio.SmartNotes.Shared.Entities.Users;

namespace Xemio.SmartNotes.Server.Abstractions.Authentication
{
    public class AuthenticationResult
    {
        #region Constructors
        /// <summary>
        /// Prevents a default instance of the <see cref="AuthenticationResult"/> class from being created.
        /// </summary>
        private AuthenticationResult()
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets a value indicating whether the authentication was successfull.
        /// </summary>
        public bool Successfull { get; set; }
        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        public string UserId { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Creates a failure instance of the <see cref="AuthenticationResult"/>.
        /// </summary>
        public static AuthenticationResult Failure()
        {
            return new AuthenticationResult
            {
                Successfull = false
            };
        }
        /// <summary>
        /// Creates a successfull instance of the <see cref="AuthenticationResult"/>.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        public static AuthenticationResult Success(string userId)
        {
            return new AuthenticationResult
            {
                Successfull = true,
                UserId = userId
            };
        }
        #endregion
    }
}
