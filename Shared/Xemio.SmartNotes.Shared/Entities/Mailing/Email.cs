using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xemio.SmartNotes.Shared.Entities.Mailing
{
    public class Email : AggregateRoot
    {
        public Email()
        {
            this.Body = new EmailBody();
            this.SendType = new EmailSendType();
            this.Attachments = new Collection<EmailAttachment>();
        }

        public string UserId { get; set; }

        public string Subject { get; set; }

        public EmailBody Body { get; set; }

        public EmailSendType SendType { get; set; }

        public ICollection<EmailAttachment> Attachments { get; set; }
    }
}
