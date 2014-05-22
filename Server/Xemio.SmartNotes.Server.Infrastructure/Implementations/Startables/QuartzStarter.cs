using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core;
using CuttingEdge.Conditions;
using Quartz;

namespace Xemio.SmartNotes.Server.Infrastructure.Implementations.Startables
{
    public class QuartzStarter : IStartable
    {
        #region Fields
        private readonly IScheduler _scheduler;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="QuartzStarter"/> class.
        /// </summary>
        /// <param name="scheduler">The scheduler.</param>
        public QuartzStarter(IScheduler scheduler)
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
            this._scheduler.Start();
        }
        /// <summary>
        /// Stops this instance.
        /// </summary>
        public void Stop()
        {
            this._scheduler.Shutdown(true);
        }
        #endregion
    }
}
