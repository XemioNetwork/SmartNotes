using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Web;
using Castle.Core.Logging;
using CuttingEdge.Conditions;
using RestSharp;
using Xemio.SmartNotes.Server.Abstractions.Mailing;
using Xemio.SmartNotes.Server.Abstractions.Services;
using Xemio.SmartNotes.Server.Infrastructure.Extensions;

namespace Xemio.SmartNotes.Server.Infrastructure.Implementations.Mailing
{
    public class MailGunEmailSender : IEmailSender
    {
        #region Fields
        private readonly RestClient  _client;
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
                BaseUrl = "https://api.mailgun.net/v2" + (customDomain == null ? "" : "/" + customDomain + "/"),
                Authenticator = new HttpBasicAuthenticator("api", apiKey)
            };
        }
        #endregion

        #region Implementation of IEmailSender
        /// <summary>
        /// Sends the specified <see cref="email" />.
        /// </summary>
        /// <param name="email">The mail.</param>
        /// <param name="deliveryDate">The date the email should be delivered.</param>
        public void Send(MailMessage email, DateTimeOffset deliveryDate)
        {
            Condition.Requires(email, "email")
                .IsNotNull();

            IRestRequest request = new RestRequest();
            request.Resource = "messages";
            request.Method = Method.POST;

            request.AddParameter("from", this.GetMailAddressString(email.From));
            request.AddParameter("to", this.GetMailAddressesString(email.To));

            if (email.CC.Any())
                request.AddParameter("cc", this.GetMailAddressesString(email.CC));

            if (email.Bcc.Any())
                request.AddParameter("bcc", this.GetMailAddressesString(email.Bcc));

            request.AddParameter("subject", email.Subject);
            request.AddParameter(email.IsBodyHtml ? "html" : "text", email.Body);
            request.AddParameter("o:deliverytime", this.GetFormattedDeliveryDate(deliveryDate));

            foreach (var attachment in email.Attachments)
            {
                var name = attachment.ContentDisposition.Inline ? "inline" : "attachment";
                request.AddFile(name, attachment.ContentStream.ToByteArray(), attachment.ContentId, attachment.ContentType.MediaType);
            }

            IRestResponse response = this._client.Execute(request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new InvalidOperationException(string.Format("Error while sending email: {0}.", response.StatusCode));
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Returns the formatted delivery date.
        /// </summary>
        /// <param name="sendDate">The send date.</param>
        private string GetFormattedDeliveryDate(DateTimeOffset sendDate)
        {
            return sendDate.ToUniversalTime().ToString(@"ddd, dd MMM yyyy HH:mm:ss G\MT", new CultureInfo("en-US"));
        }
        /// <summary>
        /// Returns the mail address string.
        /// </summary>
        /// <param name="address">The address.</param>
        private string GetMailAddressString(MailAddress address)
        {
            Condition.Requires(address, "address")
                .IsNotNull();

            if (string.IsNullOrWhiteSpace(address.DisplayName))
                return address.Address;

            return string.Format("{0} <{1}>", address.DisplayName, address.Address);
        }
        /// <summary>
        /// Returns the mail addresses string.
        /// </summary>
        /// <param name="collection">The collection.</param>
        private string GetMailAddressesString(MailAddressCollection collection)
        {
            Condition.Requires(collection, "collection")
                .IsNotNull();

            return string.Join(", ", collection.Select(this.GetMailAddressString));
        }
        #endregion
    }
}