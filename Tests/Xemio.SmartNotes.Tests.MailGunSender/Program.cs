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

            var mail = new MailMessage("info@xemio.net", "haefele@xemio.net")
            {
                Body = "Hallo"
            };

            //MailMessage mail = new MailMessage();
            //mail.Sender = new MailAddress("info@xemio.net", "Xemio Notes");
            //mail.Subject = "Hallo Welt";
            //mail.To.Add(new MailAddress("haefele@xemio.net", "Daniel Häfele"));
            //mail.IsBodyHtml = false;
            //mail.Body = "Hey du kleiner Junge!";
            
            sender.Send(mail, DateTimeOffset.Now);
        }
    }
}
