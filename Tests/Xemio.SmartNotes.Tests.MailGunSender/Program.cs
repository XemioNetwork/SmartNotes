using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Xemio.SmartNotes.Server.Infrastructure.Implementations.Mailing;

namespace Xemio.SmartNotes.Tests.MailGunSender
{
    class Program
    {
        static void Main(string[] args)
        {
            var sender = new MailGunEmailSender("key-4-h0h0rx2vmk4857p48ghlzpvgz9mck1", "xemio.net");

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("info@xemio.net", "Xemio Notes");
            mail.Subject = "Hallo Welt";
            mail.To.Add(new MailAddress("haefele@xemio.net", "Daniel Häfele"));
            mail.IsBodyHtml = true;
            mail.Body = "<img src=cid:dubbyy /> Hey du kleiner Junge!";

            var attachment = new Attachment(@"C:\Users\haefele\Desktop\image.png");
            attachment.ContentDisposition.Inline = false;
            attachment.ContentId = "dubbyy";
            attachment.ContentType.MediaType = "image/png";

            mail.Attachments.Add(attachment);

            sender.Send(mail, DateTimeOffset.Now);
        }
    }
}
