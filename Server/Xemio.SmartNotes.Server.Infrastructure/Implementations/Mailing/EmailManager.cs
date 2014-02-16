using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Logging;
using Quartz;
using Raven.Abstractions.Data;
using Raven.Client;
using Raven.Client.Document;
using Xemio.SmartNotes.Server.Abstractions.Mailing;
using Xemio.SmartNotes.Shared.Entities.Mailing;

namespace Xemio.SmartNotes.Server.Infrastructure.Implementations.Mailing
{
    public class EmailManager : IEmailManager
    {
        #region Fields
        private readonly IScheduler _scheduler;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        public ILogger Logger { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="EmailManager"/> class.
        /// </summary>
        /// <param name="scheduler">The scheduler.</param>
        public EmailManager(IScheduler scheduler)
        {
            this.Logger = NullLogger.Instance;

            this._scheduler = scheduler;

            this.InitializeJobs();
        }
        #endregion

        #region Implementation of IEmailManager
        /// <summary>
        /// Starts to send emails.
        /// </summary>
        public void StartSendingEmails()
        {
            this._scheduler.Start();

            this.Logger.Debug("Started sending emails.");
        }
        /// <summary>
        /// Stops sending the emails.
        /// </summary>
        public void Stop()
        {
            this._scheduler.Shutdown(true);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Initializes the scheduler jobs.
        /// </summary>
        private void InitializeJobs()
        {
            IJobDetail dateSpecificJob = JobBuilder.Create<DateSpecificEmailSendingJob>()
                .Build();

            ITrigger dateSpecificTrigger = TriggerBuilder.Create()
                .WithSimpleSchedule(f => f
                    .WithIntervalInMinutes(5)
                    .RepeatForever())
                .StartNow()
                .Build();

            this._scheduler.ScheduleJob(dateSpecificJob, dateSpecificTrigger);

            IJobDetail immediateJob = JobBuilder.Create<ImmediateEmailSendingJob>()
                .Build();

            ITrigger immediateTrigger = TriggerBuilder.Create()
                .WithSimpleSchedule(f => f
                    .WithIntervalInSeconds(5)
                    .RepeatForever())
                .StartNow()
                .Build();

            this._scheduler.ScheduleJob(immediateJob, immediateTrigger);
        }
        #endregion
    }
}
