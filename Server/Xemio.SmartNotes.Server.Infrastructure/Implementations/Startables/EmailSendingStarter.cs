using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core;
using CuttingEdge.Conditions;
using Quartz;
using Xemio.SmartNotes.Server.Infrastructure.Implementations.Mailing;

namespace Xemio.SmartNotes.Server.Infrastructure.Implementations.Startables
{
    public class EmailSendingStarter : IStartable
    {
        #region Fields
        private readonly IScheduler _scheduler;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="EmailSendingStarter"/> class.
        /// </summary>
        /// <param name="scheduler">The scheduler.</param>
        public EmailSendingStarter(IScheduler scheduler)
        {
            Condition.Requires(scheduler, "scheduler")
                .IsNotNull();

            this._scheduler = scheduler;
        }
        #endregion 

        #region Implementation of IStartable
        /// <summary>
        /// Starts this instance.
        /// </summary>
        public void Start()
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
        /// <summary>
        /// Stops this instance.
        /// </summary>
        public void Stop()
        {
        }
        #endregion
    }
}
