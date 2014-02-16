using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;
using Raven.Client;
using Xemio.SmartNotes.Server.Abstractions.Mailing;
using Xemio.SmartNotes.Shared.Entities.Mailing;

namespace Xemio.SmartNotes.Server.Infrastructure.Implementations.Mailing
{
    [DisallowConcurrentExecution]
    public class DateSpecificEmailSendingJob : EmailSendingJob
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DateSpecificEmailSendingJob"/> class.
        /// </summary>
        /// <param name="emailSender">The email sender.</param>
        /// <param name="documentStore">The document store.</param>
        public DateSpecificEmailSendingJob(IEmailSender emailSender, IDocumentStore documentStore) 
            : base(emailSender, documentStore)
        {
        }
        #endregion
        
        #region Overrides of EmailSendingJob
        /// <summary>
        /// Returns the emails that should be sent.
        /// </summary>
        /// <param name="session">The session.</param>
        protected override IList<Email> GetEmails(IDocumentSession session)
        {
            return session.Query<Email>()
                .Where(f => f.SendType.Immediate == false)
                .Where(f => f.SendType.AtTime <= DateTimeOffset.UtcNow)
                .Take(1024)
                .ToList();
        }
        #endregion
    }
}
