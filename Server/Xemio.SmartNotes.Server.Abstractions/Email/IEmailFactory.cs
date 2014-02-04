using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xemio.SmartNotes.Shared.Entities.Users;

namespace Xemio.SmartNotes.Server.Abstractions.Email
{
    public interface IEmailFactory
    {
        /// <summary>
        /// Creates the "Password forgot" email.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="url">The url to reset the password.</param>
        IEmail CreatePasswordForgotEmail(User user, string url);
        /// <summary>
        /// Creates the "Password reset" email.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="newPassword">The new password.</param>
        IEmail CreatePasswordResetEmail(User user, string newPassword);
    }
}
