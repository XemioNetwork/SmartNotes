using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Logging;
using Xemio.SmartNotes.Abstractions.Common;
using Xemio.SmartNotes.Server.Abstractions.Email;

namespace Xemio.SmartNotes.Server.Infrastructure.Implementations.Email
{
    /// <summary>
    /// A implementation of <see cref="IEmailSender"/> using the .NET <see cref="SmtpClient"/>.
    /// </summary>
    public class SmtpEmailSender : IEmailSender
    {
        #region Fields
        private readonly BackgroundQueue<IEmail> _emailQueue;
        private readonly SmtpClient _smtpClient;

        private ILogger _logger = NullLogger.Instance;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        public ILogger Logger
        {
            get { return this._logger; }
            set { this._logger = value; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="SmtpEmailSender"/> class.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="port">The port.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        public SmtpEmailSender(string host, int port, string username, string password)
        {
            this._emailQueue = new BackgroundQueue<IEmail>(this.SendMail, this.OnException);
            this._smtpClient = new SmtpClient
                               {
                                   Host = host,
                                   Port = port
                               };

            if (string.IsNullOrWhiteSpace(username) == false)
            {
                this._smtpClient.Credentials = new NetworkCredential(username, password);
            }
        }
        #endregion

        #region Implementation Of IEmailSender
        /// <summary>
        /// Sends the specified mail.
        /// </summary>
        /// <param name="mail">The mail.</param>
        public void SendAsync(IEmail mail)
        {
            this._emailQueue.Enqueue(mail);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Sends the mail.
        /// </summary>
        /// <param name="mail">The mail.</param>
        private void SendMail(IEmail mail)
        {
            MailMessage message = this.GetMailMessage(mail);
            this._smtpClient.Send(message);

            this.Logger.Debug(string.Format("Sending email. {0}From: {1}{0}To: {2}{0}Subject: {3}{0}Body: {4}",
                                            Environment.NewLine,
                                            message.From.Address,
                                            message.To,
                                            message.Subject,
                                            message.Body));
        }

        /// <summary>
        /// Called when an exception happens.
        /// </summary>
        /// <param name="item">The item responsible for the exception.</param>
        /// <param name="exception">The exception.</param>
        private bool OnException(IEmail item, Exception exception)
        {
            this.Logger.Error("Exception while sending mail.", exception);
            return true;
        }
        /// <summary>
        /// Extracts the <see cref="MailMessage"/> from the given <see cref="IEmail"/>.
        /// Throws an <see cref="InvalidOperationException"/> if the given <see cref="IEmail"/> is no <see cref="SmtpEmail"/>.
        /// </summary>
        /// <param name="email">The email.</param>
        private MailMessage GetMailMessage(IEmail email)
        {
            var smtpEmail = email as SmtpEmail;

            if (smtpEmail == null)
                throw new InvalidOperationException("The SmptEmailSender can only send SmtpEmails.");

            return smtpEmail.MailMessage;
        }
        #endregion
    }
}
