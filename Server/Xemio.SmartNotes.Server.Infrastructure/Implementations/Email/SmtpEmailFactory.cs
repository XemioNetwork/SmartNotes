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
        private readonly string _senderEmailAddress;
        private readonly string _senderName;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="SmtpEmailFactory"/> class.
        /// </summary>
        /// <param name="senderEmailAddress">The sender email address.</param>
        /// <param name="senderName">The sender name.</param>
        public SmtpEmailFactory(string senderEmailAddress, string senderName)
        {
            this._senderEmailAddress = senderEmailAddress;
            this._senderName = senderName;
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
            SmtpEmail email = this.CreateNewEmail();

            email.MailMessage.To.Add(new MailAddress(user.EmailAddress));
            email.MailMessage.Subject = EmailMessages.PasswordForgotSubject;
            email.MailMessage.Body = string.Format(EmailMessages.PasswordForgotBody, user.Username, url);

            return email;
        }
        /// <summary>
        /// Creates the "Password reset" email.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="newPassword">The new password.</param>
        public IEmail CreatePasswordResetEmail(User user, string newPassword)
        {
            SmtpEmail email = this.CreateNewEmail();

            email.MailMessage.To.Add(new MailAddress(user.EmailAddress));
            email.MailMessage.Subject = EmailMessages.PasswordResetSubject;
            email.MailMessage.Body = string.Format(EmailMessages.PasswordResetBody, user.Username, newPassword);

            return email;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Creates a new email.
        /// </summary>
        private SmtpEmail CreateNewEmail()
        {
            var email = new SmtpEmail
                        {
                            MailMessage =
                            {
                                From = new MailAddress(this._senderEmailAddress, this._senderName)
                            }
                        };

            return email;
        }
        #endregion
    }
}
