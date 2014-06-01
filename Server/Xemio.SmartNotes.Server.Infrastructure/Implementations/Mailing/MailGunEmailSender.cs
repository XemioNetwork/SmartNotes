using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Web;

using CuttingEdge.Conditions;
using RestSharp;
using Xemio.SmartNotes.Server.Abstractions.Mailing;

namespace Xemio.SmartNotes.Server.Infrastructure.Implementations.Mailing
{
    public class MailGunEmailSender : IEmailSender
    {
        #region Fields
        private readonly RestClient  _client;
        private readonly MailWriter _mailWriter;
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

            this._mailWriter = new MailWriter();
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
            
            IRestResponse response = this._client.Execute(request);

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

    public class MailWriter
    {
        #region Fields
        private readonly BindingFlags _flags;
        private readonly ConstructorInfo _mailWriterContructor;
        private readonly MethodInfo _sendMethod;
        private readonly MethodInfo _closeMethod;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="MailWriter"/> class.
        /// </summary>
        public MailWriter()
        {
            this._flags = BindingFlags.Instance | BindingFlags.NonPublic;

            this._mailWriterContructor = this.GetMailWriterConstructor();
            this._sendMethod = this.GetSendMethod();
            this._closeMethod = this.GetCloseMethod();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Writes the specified mail message.
        /// </summary>
        /// <param name="mailMessage">The mail message.</param>
        public string Write(MailMessage mailMessage)
        {
            using (var stream = new MemoryStream())
            {
                object mailWriter = this._mailWriterContructor.Invoke(new[] { stream });

                this._sendMethod.Invoke(mailMessage, _flags, null, new[] { mailWriter, true, true }, null);
                this._closeMethod.Invoke(mailWriter, _flags, null, new object[0], null);


                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Returns the mail writer constructor.
        /// </summary>
        private ConstructorInfo GetMailWriterConstructor()
        {
            Assembly assembly = typeof(SmtpClient).Assembly;
            Type mailWriterType = assembly.GetType("System.Net.Mail.MailWriter");

            return mailWriterType.GetConstructor(
                    _flags,
                    null,
                    new[] { typeof(Stream) },
                    null);
        }
        /// <summary>
        /// Returns the send method.
        /// </summary>
        private MethodInfo GetSendMethod()
        {
            return typeof(MailMessage).GetMethod(
                        "Send",
                        _flags);
        }
        /// <summary>
        /// Returns the close method.
        /// </summary>
        private MethodInfo GetCloseMethod()
        {
            Assembly assembly = typeof(SmtpClient).Assembly;
            Type mailWriterType = assembly.GetType("System.Net.Mail.MailWriter");

            return mailWriterType.GetMethod(
                        "Close",
                        _flags);
        }
        #endregion
    }
}