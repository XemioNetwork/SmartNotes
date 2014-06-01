using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Logging;
using CuttingEdge.Conditions;
using NodaTime;
using Raven.Abstractions.Extensions;
using Raven.Client;
using Xemio.SmartNotes.Server.Abstractions.Mailing;
using Xemio.SmartNotes.Server.Abstractions.Services;
using Xemio.SmartNotes.Server.Infrastructure.Extensions;
using Xemio.SmartNotes.Shared.Entities.Mailing;
using Xemio.SmartNotes.Shared.Entities.Users;

namespace Xemio.SmartNotes.Server.Infrastructure.Implementations.Mailing
{
    public class EmailFactory : IEmailFactory
    {
        #region Fields
        private readonly IDocumentStore _documentStore;
        private readonly IEmailSender _emailSender;
        private readonly IFileService _fileService;
        private readonly EmailPerson _sender;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        public ILogger Logger { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="EmailFactory"/> class.
        /// </summary>
        /// <param name="documentStore">The document store.</param>
        /// <param name="emailSender">The email sender.</param>
        /// <param name="fileService">The file service.</param>
        /// <param name="sender">The sender.</param>
        public EmailFactory(IDocumentStore documentStore, IEmailSender emailSender, IFileService fileService, EmailPerson sender)
        {
            Condition.Requires(documentStore, "documentStore")
                .IsNotNull();
            Condition.Requires(sender, "emailSender")
                .IsNotNull();
            Condition.Requires(fileService, "fileService")
                .IsNotNull();
            Condition.Requires(sender, "sender")
                .IsNotNull();

            this.Logger = NullLogger.Instance;

            this._documentStore = documentStore;
            this._emailSender = emailSender;
            this._fileService = fileService;
            this._sender = sender;
        }
        #endregion

        #region Implementation of IEmailFactory
        /// <summary>
        /// Sends an email to the specified <paramref name="user"/>.
        /// It will use the template specified in the <paramref name="mailTemplateName"/> and replace it's variables with the <paramref name="additionalData"/>.
        /// </summary>
        /// <param name="mailTemplateName">Name of the mail template.</param>
        /// <param name="user">The user.</param>
        /// <param name="additionalData">The additional data.</param>
        public void SendEmailToUser(string mailTemplateName, User user, dynamic additionalData)
        {
            Condition.Requires(mailTemplateName, "mailTemplateName")
                .IsNotNullOrWhiteSpace();
            Condition.Requires(user, "user")
                .IsNotNull();

            using (IDocumentSession session = this._documentStore.OpenSession())
            { 
                //Get the email template
                EmailTemplate emailTemplate = this.GetEmailTemplateWithName(mailTemplateName, session);
                if (emailTemplate == null)
                {
                    this.Logger.ErrorFormat("Tried sending mail '{0}' to user '{1}', but no template for this mail was found.", mailTemplateName, user.Id);
                    return;
                }
            
                //Get the localized texts
                EmailTemplateTexts texts = this.GetEmailTemplateTexts(emailTemplate, user.PreferredLanguage, session);

                MailMessage mail = this.CreateMailMessage(emailTemplate, texts, user, additionalData);
                DateTimeOffset sendDate = this.GetSendTime(emailTemplate, user);

                 this._emailSender.Send(mail, sendDate);
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Returns the <see cref="EmailTemplate"/> for the specified <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="session">The session.</param>
        private EmailTemplate GetEmailTemplateWithName(string name, IDocumentSession session)
        {
            Condition.Requires(name, "name")
                .IsNotNullOrWhiteSpace();
            Condition.Requires(session, "session")
                .IsNotNull();

            string documentId = session.Advanced.DocumentStore.Conventions.FindFullDocumentKeyFromNonStringIdentifier(name, typeof(EmailTemplate), false);

            return session.Load<EmailTemplate>(documentId);
        }
        /// <summary>
        /// Returns the <see cref="EmailTemplateTexts"/> in the specified <paramref name="language"/>.
        /// </summary>
        /// <param name="template">The template.</param>
        /// <param name="language">The language.</param>
        /// <param name="session">The session.</param>
        private EmailTemplateTexts GetEmailTemplateTexts(EmailTemplate template, string language, IDocumentSession session)
        {
            Condition.Requires(template, "template")
                .IsNotNull();
            Condition.Requires(language, "language")
                .IsNotNullOrWhiteSpace();
            Condition.Requires(session, "session")
                .IsNotNull();

            string documentId = string.Format("{0}/{1}", template.Id, language);

            return session.Load<EmailTemplateTexts>(documentId);
        }
        /// <summary>
        /// Creates the mail message.
        /// </summary>
        /// <param name="emailTemplate">The email template.</param>
        /// <param name="texts">The texts.</param>
        /// <param name="user">The user.</param>
        /// <param name="additionalData">The additional data.</param>
        private MailMessage CreateMailMessage(EmailTemplate emailTemplate, EmailTemplateTexts texts, User user, object additionalData)
        {
            Condition.Requires(emailTemplate, "emailTEmplate")
                .IsNotNull();
            Condition.Requires(texts, "texts")
                .IsNotNull();
            Condition.Requires(user, "user")
                .IsNotNull();
            Condition.Requires(additionalData, "additionalData")
                .IsNotNull();
            
            var mailMessage = new MailMessage
            {
                From = new MailAddress(this._sender.Address, this._sender.Name),
                Subject = texts.Subject,
                IsBodyHtml = texts.Body.IsHtml,
                Body = texts.Body.Content.FormatWith((object)additionalData)
            };
            mailMessage.To.Add(new MailAddress(user.EmailAddress, user.EmailAddress));

            foreach (var emailAttachment in emailTemplate.Attachments)
            {
                string filePath = this._fileService.GetFullPath(emailAttachment.FilePath);

                var attachment = new Attachment(filePath);
                attachment.ContentDisposition.Inline = emailAttachment.IsInline;
                attachment.ContentDisposition.DispositionType = emailAttachment.IsInline ? DispositionTypeNames.Inline : DispositionTypeNames.Attachment;
                attachment.ContentId = emailAttachment.Name;
                attachment.ContentType.MediaType = emailAttachment.MediaType;
                attachment.ContentType.Name = emailAttachment.Name;

                mailMessage.Attachments.Add(attachment);
            }

            return mailMessage;
        }
        /// <summary>
        /// Returns the send time for the specified <paramref name="template"/> considering the <see cref="user"/>s time zone.
        /// </summary>
        /// <param name="template">The template.</param>
        /// <param name="user">The user.</param>
        private DateTimeOffset GetSendTime(EmailTemplate template, User user)
        {
            Condition.Requires(template, "template")
                .IsNotNull();
            Condition.Requires(user, "user")
                .IsNotNull();

            if (template.SendImmediate)
                return DateTimeOffset.UtcNow;

            var timeZone = DateTimeZoneProviders.Tzdb[user.TimeZoneId];

            //We check in the User's timezone if it's after 2 PM
            ZonedDateTime sendDate = timeZone.AtLeniently(LocalDateTime.FromDateTime(DateTime.Now));
            if (sendDate.Hour > 14)
            {
                //Add a day if it's after 2 PM
                sendDate = sendDate.Plus(Duration.FromStandardDays(1));
            }

            //Use the date information from the sendDate
            var sendDateWithTime = new DateTime(sendDate.Year, sendDate.Month, sendDate.Day, 10, 0, 0);

            //Convert the DateTime to the users timezone
            return timeZone.AtLeniently(LocalDateTime.FromDateTime(sendDateWithTime)).ToDateTimeOffset();
        }
        #endregion
    }
}
