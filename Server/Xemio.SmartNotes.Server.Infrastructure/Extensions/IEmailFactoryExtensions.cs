using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Xemio.SmartNotes.Server.Abstractions.Mailing;
using Xemio.SmartNotes.Shared.Entities.Users;

namespace Xemio.SmartNotes.Server.Infrastructure.Extensions
{
    public static class IEmailFactoryExtensions
    {
        /// <summary>
        /// Sends a "PasswordForgot" email to the user.
        /// </summary>
        /// <param name="emailFactory">The email factory.</param>
        /// <param name="user">The user.</param>
        /// <param name="url">The URL to reset the password.</param>
        public static void SendPasswordForgotEmail(this IEmailFactory emailFactory, User user, string url)
        {
            emailFactory.SendEmailToUser("PasswordForgot", user, new
            {
                Url = url,
                user.Username
            });
        }

        /// <summary>
        /// Sends a "PasswordReset" email to the user.
        /// </summary>
        /// <param name="emailFactory">The email factory.</param>
        /// <param name="user">The user.</param>
        /// <param name="newPassword">The new password.</param>
        public static void SendPasswordResetEmail(this IEmailFactory emailFactory, User user, string newPassword)
        {
            emailFactory.SendEmailToUser("PasswordReset", user, new
            {
                NewPassword = newPassword,
                user.Username
            });
        }
    }
}
