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
using System.Threading;
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
        private readonly ThreadLocal<SmtpClient> _smtpClient;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        public ILogger Logger { get; set; }
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
            Condition.Requires(host, "host")
                .IsNotNullOrWhiteSpace();
            Condition.Requires(port, "port")
                .IsNotLessOrEqual(0);
            Condition.Requires(username, "username")
                .IsNotNull();
            Condition.Requires(password, "password")
                .IsNotNull();

            this.Logger = NullLogger.Instance;

            this._smtpClient = new ThreadLocal<SmtpClient>(() => new SmtpClient
                               {
                                   Host = host,
                                   Port = port,
                                   Credentials = string.IsNullOrWhiteSpace(username) ? null : new NetworkCredential(username, password)
                               });
        }
        #endregion

        #region Implementation Of IEmailSender
        /// <summary>
        /// Sends the asynchronous.
        /// </summary>
        /// <param name="mail">The mail.</param>
        /// <param name="deliveryDate">The send date.</param>
        public void Send(MailMessage mail, DateTimeOffset deliveryDate)
        {
            this._smtpClient.Value.Send(mail);
        }
        #endregion
    }
}
