using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using CuttingEdge.Conditions;
using RestSharp;
using Xemio.SmartNotes.Server.Abstractions.Mailing;

namespace Xemio.SmartNotes.Server.Infrastructure.Implementations.Mailing
{
    public class MailGunEmailSender : IEmailSender
    {
        #region Fields
        private readonly RestClient _client;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="MailGunEmailSender"/> class.
        /// </summary>
        /// <param name="apiKey">The API key.</param>
        /// <param name="customDomain">The custom domain.</param>
        public MailGunEmailSender(string apiKey, string customDomain = null)
        {
            Condition.Requires(apiKey, "apiKey")
                .IsNotNull();

            this._client = new RestClient
            {
                BaseUrl = "https://api.mailgun.net/v2" + (customDomain == null ? "" : "/" + customDomain),
                Authenticator = new HttpBasicAuthenticator("api", apiKey)
            };
        }
        #endregion

        #region Implementation of IEmailSender
        /// <summary>
        /// Sends the specified <see cref="email" />.
        /// </summary>
        /// <param name="email">The mail.</param>
        /// <param name="sendDate">The date the email should be sent.</param>
        public void Send(MailMessage email, DateTimeOffset sendDate)
        {
            var request = new RestRequest("messages", Method.POST);
            
            request.AddParameter("from", "Excited User <me@samples.mailgun.org>");
            request.AddParameter("to", "foo@example.com");
            request.AddParameter("cc", "baz@example.com");
            request.AddParameter("bcc", "bar@example.com");

            request.AddParameter("subject", "Hello");
            request.AddParameter("text", "Testing some Mailgun awesomness!");
            request.AddParameter("html", "<html>HTML version of the body</html>");

            foreach (var a in email.Attachments)
            {
                request.AddFile()
            }

            request.AddFile("attachment", Path.Combine("files", "test.jpg"));
            request.AddFile("attachment", Path.Combine("files", "test.txt"));

        }
        #endregion
    }
}