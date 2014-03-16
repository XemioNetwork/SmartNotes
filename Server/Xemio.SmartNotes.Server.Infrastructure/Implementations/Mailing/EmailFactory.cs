using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Logging;
using NodaTime;
using Raven.Abstractions.Extensions;
using Raven.Client;
using Xemio.SmartNotes.Server.Abstractions.Mailing;
using Xemio.SmartNotes.Server.Infrastructure.Extensions;
using Xemio.SmartNotes.Shared.Entities.Mailing;
using Xemio.SmartNotes.Shared.Entities.Users;

namespace Xemio.SmartNotes.Server.Infrastructure.Implementations.Mailing
{
    public class EmailFactory : IEmailFactory
    {
        #region Fields
        private readonly IDocumentStore _documentStore;
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
        public EmailFactory(IDocumentStore documentStore)
        {
            this.Logger = NullLogger.Instance;

            this._documentStore = documentStore;
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
            if (string.IsNullOrWhiteSpace(mailTemplateName))
                throw new ArgumentNullException("mailTemplateName");
            if (user == null)
                throw new ArgumentNullException("user");

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

                //Create the mail
                var email = new Email
                {
                    Attachments = emailTemplate.Attachments,
                    Body =
                    {
                        Content = texts.Body.Content.FormatWith((object)additionalData),
                        IsHtml = texts.Body.IsHtml
                    },
                    Subject = texts.Subject,
                    UserId = user.Id,
                    SendType =
                    {
                        Immediate = emailTemplate.SendImmediate,
                        AtTime = emailTemplate.SendImmediate ? (DateTimeOffset?)null : this.GetSendTime(emailTemplate, user)
                    }
                };

                //Store it so it will be sent 
                session.Store(email);

                session.SaveChanges();
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
            string documentId = string.Format("{0}/{1}", template.Id, language);

            return session.Load<EmailTemplateTexts>(documentId);
        }
        /// <summary>
        /// Returns the send time for the specified <paramref name="template"/> considering the <see cref="user"/>s time zone.
        /// </summary>
        /// <param name="template">The template.</param>
        /// <param name="user">The user.</param>
        private DateTimeOffset GetSendTime(EmailTemplate template, User user)
        {
            if (template == null)
                throw new ArgumentNullException("template");
            if (user == null)
                throw new ArgumentNullException("user");

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
