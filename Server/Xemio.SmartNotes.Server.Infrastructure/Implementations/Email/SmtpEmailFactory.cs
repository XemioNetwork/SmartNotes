using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Xemio.SmartNotes.Models.Entities.Users;
using Xemio.SmartNotes.Server.Abstractions.Email;
using Xemio.SmartNotes.Server.Infrastructure.Implementations.Email.Resources;

namespace Xemio.SmartNotes.Server.Infrastructure.Implementations.Email
{
    public class SmtpEmailFactory : IEmailFactory
    {
        #region Fields
        private readonly string _infoEmailAddress;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="SmtpEmailFactory"/> class.
        /// </summary>
        /// <param name="infoEmailAddress">The info email address.</param>
        public SmtpEmailFactory(string infoEmailAddress)
        {
            this._infoEmailAddress = infoEmailAddress;
        }
        #endregion

        #region Implementation of IEmailFactory
        /// <summary>
        /// Creates the "Password forgot" email.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="url">The url to reset the password.</param>
        public IEmail CreatePasswordForgotEmail(User user, string url)
        {
            var email = new SmtpEmail();

            email.MailMessage.To.Add(new MailAddress(user.EmailAddress));
            email.MailMessage.Subject = EmailMessages.PasswordForgotSubject;
            email.MailMessage.Body = string.Format(EmailMessages.PasswordForgotBody, user.Username, url);
            email.MailMessage.From = new MailAddress(this._infoEmailAddress);

            return email;
        }
        /// <summary>
        /// Creates the "Password reset" email.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="newPassword">The new password.</param>
        public IEmail CreatePasswordResetEmail(User user, string newPassword)
        {
            var email = new SmtpEmail();

            email.MailMessage.To.Add(new MailAddress(user.EmailAddress));
            email.MailMessage.Subject = EmailMessages.PasswordResetSubject;
            email.MailMessage.Body = string.Format(EmailMessages.PasswordResetBody, user.Username, newPassword);
            email.MailMessage.From = new MailAddress(this._infoEmailAddress);

            return email;
        }
        #endregion
    }
}
