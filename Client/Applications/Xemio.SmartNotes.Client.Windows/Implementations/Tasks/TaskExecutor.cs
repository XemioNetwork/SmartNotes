using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;
using Xemio.SmartNotes.Abstractions.Common;
using Xemio.SmartNotes.Client.Abstractions.Tasks;
using Xemio.SmartNotes.Client.Windows.Data.Events;

namespace Xemio.SmartNotes.Client.Windows.Implementations.Tasks
{
    public class TaskExecutor : ITaskExecutor, IDisposable
    {
        #region Fields
        private readonly IEventAggregator _eventAggregator;
        private readonly BackgroundQueue<ITask> _taskQueue;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskExecutor" /> class.
        /// </summary>
        /// <param name="eventAggregator">The event aggregator.</param>
        public TaskExecutor(IEventAggregator eventAggregator)
        {
            this._eventAggregator = eventAggregator;
            this._taskQueue = new BackgroundQueue<ITask>(this.Execute);
        }
        #endregion

        #region Implementation of ITaskExecutor
        /// <summary>
        /// Gets the current task.
        /// </summary>
        public ITask CurrentTask { get; private set; }
        /// <summary>
        /// Starts the task.
        /// </summary>
        /// <param name="task">The task.</param>
        public void StartTask(ITask task)
        {
            this._taskQueue.Enqueue(task);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Executes the specified task.
        /// </summary>
        /// <param name="task">The task.</param>
        private void Execute(ITask task)
        {
            this.CurrentTask = task;
            this._eventAggregator.Publish(new CurrentTaskChangedEvent(this.CurrentTask));

            task.Execute().Wait();
        }
        #endregion

        #region Implementation of IDisposable
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this._taskQueue.Dispose();
        }
        #endregion
    }
}
