using System;

namespace Xemio.SmartNotes.Shared.Entities.Mailing
{
    public class EmailTemplateTexts : AggregateRoot
    {
        public string Subject { get; set; }
        public EmailBody Body { get; set; }
    }
}