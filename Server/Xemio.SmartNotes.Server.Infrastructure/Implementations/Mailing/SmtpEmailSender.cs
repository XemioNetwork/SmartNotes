using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Logging;
using CuttingEdge.Conditions;
using Raven.Client;
using Xemio.SmartNotes.Server.Abstractions.Mailing;
using Xemio.SmartNotes.Server.Abstractions.Services;
using Xemio.SmartNotes.Shared.Common;
using Xemio.SmartNotes.Shared.Entities.Mailing;
using Xemio.SmartNotes.Shared.Entities.Users;

namespace Xemio.SmartNotes.Server.Infrastructure.Implementations.Mailing
{
    /// <summary>
    /// A implementation of <see cref="IEmailSender"/> using the .NET <see cref="SmtpClient"/>.
    /// </summary>
    public class SmtpEmailSender : IEmailSender
    {
        #region Fields
        private readonly IFileService _fileService;
        private readonly EmailPerson _sender;

        private readonly BackgroundQueue<Tuple<SentEmail, TaskCompletionSource<bool>>> _emailQueue;
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
        /// <param name="fileService">The file service.</param>
        /// <param name="sender">The email sender.</param>
        public SmtpEmailSender(string host, int port, string username, string password, IFileService fileService, EmailPerson sender)
        {
            Condition.Requires(host, "host")
                .IsNotNullOrWhiteSpace();
            Condition.Requires(port, "port")
                .IsNotLessOrEqual(0);
            Condition.Requires(username, "username")
                .IsNotNull();
            Condition.Requires(password, "password")
                .IsNotNull();
            Condition.Requires(fileService, "fileService")
                .IsNotNull();
            Condition.Requires(sender, "sender")
                .IsNotNull();

            this._fileService = fileService;
            this._sender = sender;

            this._emailQueue = new BackgroundQueue<Tuple<SentEmail, TaskCompletionSource<bool>>>(this.SendMail);
            this._emailQueue.UnhandledExceptionEvent += OnException;
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
        public Task SendAsync(SentEmail mail)
        {
            var completionSource = new TaskCompletionSource<bool>();
            this._emailQueue.Enqueue(Tuple.Create(mail, completionSource));

            return completionSource.Task;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Sends the mail.
        /// </summary>
        /// <param name="mail">The mail.</param>
        private void SendMail(Tuple<SentEmail, TaskCompletionSource<bool>>  mail)
        {
            mail.Item1.Sender = this._sender;
            mail.Item1.SentDate = DateTimeOffset.UtcNow;

            MailMessage message = this.GetMailMessage(mail.Item1);

            this._smtpClient.Send(message);

            this.Logger.Debug(string.Format("Sending email. {0}From: {1}{0}To: {2}{0}Subject: {3}{0}Body: {4}",
                                            Environment.NewLine,
                                            message.From.Address,
                                            message.To,
                                            message.Subject,
                                            message.Body));

            mail.Item2.SetResult(true);
        }
        /// <summary>
        /// Called when an exception happens while sending an email.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="backgroundExceptionEventArgs">The background exception event arguments.</param>
        private void OnException(object sender, BackgroundExceptionEventArgs<Tuple<SentEmail, TaskCompletionSource<bool>>> backgroundExceptionEventArgs)
        {
            this.Logger.Error("Exception while sending mail.", backgroundExceptionEventArgs.Exception);

            backgroundExceptionEventArgs.Item.Item2.SetException(backgroundExceptionEventArgs.Exception);
        }
        /// <summary>
        /// Extracts the <see cref="MailMessage"/> from the given <see cref="Email"/>.
        /// </summary>
        /// <param name="email">The email.</param>
        private MailMessage GetMailMessage(SentEmail email)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(email.Sender.Address, email.Sender.Name),
                Subject = email.Subject,
                IsBodyHtml = email.Body.IsHtml,
                Body = email.Body.Content
            };
            mailMessage.To.Add(new MailAddress(email.Receiver.Address, email.Receiver.Name));

            foreach (var emailAttachment in email.Attachments)
            {
                string filePath = this._fileService.GetFullPath(emailAttachment.FilePath);

                var attachment = new Attachment(filePath);
                attachment.ContentDisposition.Inline = emailAttachment.IsInline;
                attachment.ContentDisposition.DispositionType = emailAttachment.IsInline ? DispositionTypeNames.Inline : DispositionTypeNames.Attachment;
                attachment.ContentId = emailAttachment.Name;
                attachment.ContentType.MediaType = emailAttachment.MediaType;
                attachment.ContentType.Name = emailAttachment.Name;

                mailMessage.Attachments.Add(attachment);
            }

            return mailMessage;
        }
        #endregion
    }
}
