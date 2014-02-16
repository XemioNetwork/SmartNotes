using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xemio.SmartNotes.Shared.Entities.Mailing
{
    public class SentEmail : AggregateRoot
    {
        public EmailPerson Sender { get; set; }
        public EmailPerson Receiver { get; set; }
        public string UserId { get; set; }
        public string Subject { get; set; }
        public EmailBody Body { get; set; }
        public ICollection<EmailAttachment> Attachments { get; set; }
        public DateTimeOffset SentDate { get; set; }
    }
}
