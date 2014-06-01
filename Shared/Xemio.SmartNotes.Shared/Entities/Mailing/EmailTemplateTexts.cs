using System;

namespace Xemio.SmartNotes.Shared.Entities.Mailing
{
    public class EmailTemplateTexts : AggregateRoot
    {
        public string Subject { get; set; }
        public bool IsContentHtml { get; set; }
        public string Content { get; set; }
    }
}