using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Xemio.SmartNotes.Server.Abstractions.Email
{
    public interface IEmailSender
    {
        /// <summary>
        /// Sends the specified mail.
        /// </summary>
        /// <param name="mail">The mail.</param>
        void SendAsync(IEmail mail);
    }
}
