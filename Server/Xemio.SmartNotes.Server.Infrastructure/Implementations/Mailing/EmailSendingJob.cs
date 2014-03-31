using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Logging;
using Quartz;
using Raven.Abstractions.Data;
using Raven.Client;
using Xemio.SmartNotes.Server.Abstractions.Mailing;
using Xemio.SmartNotes.Shared.Entities.Mailing;
using Xemio.SmartNotes.Shared.Entities.Users;

namespace Xemio.SmartNotes.Server.Infrastructure.Implementations.Mailing
{
    public abstract class EmailSendingJob : IJob
    {
        #region Fields
        private readonly IEmailSender _emailSender;
        private readonly IDocumentStore _documentStore;
        #endregion
        
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="EmailSendingJob"/> class.
        /// </summary>
        /// <param name="emailSender">The email sender.</param>
        /// <param name="documentStore">The document store.</param>
        protected EmailSendingJob(IEmailSender emailSender, IDocumentStore documentStore)
        {
            this._emailSender = emailSender;
            this._documentStore = documentStore;
        }
        #endregion

        #region Implementation of IJob
        /// <summary>
        /// Called by the <see cref="T:Quartz.IScheduler" /> when a <see cref="T:Quartz.ITrigger" />
        /// fires that is associated with the <see cref="T:Quartz.IJob" />.
        /// </summary>
        /// <param name="context">The execution context.</param>
        public void Execute(IJobExecutionContext context)
        {
            using (var session = this._documentStore.OpenSession())
            {
                //Load all emails that need to sent
                var emails = this.GetEmails(session);

                foreach(var email in emails)
                {
                    SentEmail sentEmail = this.ConvertEmail(email, session);

                    //This is intentionally synchronous, because if we use ContinueWith or await, we would already return control to the Quartz-Scheduler
                    //And if that happens, this job might get called again.
                    //The result is: Emails will be sent twice.
                    //So we make it synchronous and it's fine.
                    this._emailSender.SendAsync(sentEmail).Wait();

                    //Delete the email
                    session.Delete(email);

                    //Store it as sent
                    session.Store(sentEmail);
                }

                session.SaveChanges();
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns the emails that should be sent.
        /// </summary>
        /// <param name="session">The session.</param>
        protected abstract IList<Email> GetEmails(IDocumentSession session);
        #endregion

        #region Private Methods

        /// <summary>
        /// Converts the specified <paramref name="email"/> into an <see cref="SentEmail"/>.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="session">The session.</param>
        private SentEmail ConvertEmail(Email email, IDocumentSession session)
        {
            var user = session.Load<User>(email.UserId);

            return new SentEmail
            {
                Attachments = email.Attachments,
                Receiver = new EmailPerson
                {
                    Name = user.EmailAddress,
                    Address = user.EmailAddress
                },
                Subject = email.Subject,
                Body = email.Body,
                UserId = email.UserId
            };
        }
        #endregion
    }
}
