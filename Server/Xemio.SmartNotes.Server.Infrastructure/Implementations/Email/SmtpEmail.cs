using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Xemio.SmartNotes.Server.Abstractions.Email;

namespace Xemio.SmartNotes.Server.Infrastructure.Implementations.Email
{
    public class SmtpEmail : IEmail
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="SmtpEmail"/> class.
        /// </summary>
        public SmtpEmail()
        {
            this.MailMessage = new MailMessage();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the mail message.
        /// </summary>
        public MailMessage MailMessage { get; set; }
        #endregion
    }
}
