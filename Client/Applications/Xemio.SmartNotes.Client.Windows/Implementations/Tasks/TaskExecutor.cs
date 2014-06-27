using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using Caliburn.Micro;
using Castle.Core.Logging;
using Xemio.SmartNotes.Client.Shared.Tasks;
using Xemio.SmartNotes.Client.Windows.Data.Events;
using Xemio.SmartNotes.Client.Windows.Data.Exceptions;
using Xemio.SmartNotes.Client.Windows.Implementations.Interaction;
using Xemio.SmartNotes.Shared.Common;

namespace Xemio.SmartNotes.Client.Windows.Implementations.Tasks
{
    public class TaskExecutor : ITaskExecutor, IDisposable
    {
        #region Fields
        private readonly IEventAggregator _eventAggregator;
        private readonly DisplayManager _displayManager;

        private readonly BackgroundQueue<ITask> _taskQueue;
        private readonly ConcurrentDictionary<ITask, bool> _canceledTasks; 
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        public ILogger Logger { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskExecutor" /> class.
        /// </summary>
        /// <param name="eventAggregator">The event aggregator.</param>
        /// <param name="displayManager">The display manager.</param>
        public TaskExecutor(IEventAggregator eventAggregator, DisplayManager displayManager)
        {
            this.Logger = NullLogger.Instance;

            this._eventAggregator = eventAggregator;
            this._displayManager = displayManager;

            this._taskQueue = new BackgroundQueue<ITask>(this.Execute);
            this._taskQueue.UnhandledExceptionEvent += OnException;

            this._canceledTasks = new ConcurrentDictionary<ITask, bool>();
        }
        #endregion

        #region Implementation of ITaskExecutor
        /// <summary>
        /// Gets the current task.
        /// </summary>
        public ITask CurrentTask { get; private set; }
        /// <summary>
        /// Gets the tasks.
        /// </summary>
        public IReadOnlyCollection<ITask> Tasks
        {
            get { return this._taskQueue.Items; }
        }
        /// <summary>
        /// Starts the task.
        /// </summary>
        /// <param name="task">The task.</param>
        public void StartTask(ITask task)
        {
            task.StartDate = DateTimeOffset.Now;

            this._taskQueue.Enqueue(task);

            this._eventAggregator.PublishOnUIThread(new TaskStartedEvent(task));
        }
        /// <summary>
        /// Cancels the task. Returns whether it was canceled successfully.
        /// </summary>
        /// <param name="task">The task.</param>
        public bool CancelTask(ITask task)
        {
            if (this._taskQueue.Items.Contains(task) == false)
                return false;

            if (this.CurrentTask == task)
                return false;

            this._canceledTasks.AddOrUpdate(task, true, (t, b) => true);

            return true;
        }
        /// <summary>
        /// Determines whether this instance has tasks.
        /// </summary>
        public bool HasTasks()
        {
            //We have tasks in the queue or we are executing a task
            return this._taskQueue.Count > 0 || this.CurrentTask != null;
        }
        /// <summary>
        /// Cancels the execution.
        /// </summary>
        public void CancelExecution()
        {
            this._taskQueue.Dispose();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Executes the specified task.
        /// </summary>
        /// <param name="task">The task.</param>
        private void Execute(ITask task)
        {
            if (this._canceledTasks.GetOrAdd(task, false) == true)
            {
                bool canceled;
                this._canceledTasks.TryRemove(task, out canceled);

                return;
            }

            this.CurrentTask = task;
            this._eventAggregator.PublishOnUIThread(new TaskExecutingEvent(task));

            task.Execute().Wait();

            this.CurrentTask = null;
            this._eventAggregator.PublishOnUIThread(new TaskExecutedEvent(task));
        }
        /// <summary>
        /// Called when an exception happens while executing a task.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">The event arguments.</param>
        private void OnException(object sender, BackgroundExceptionEventArgs<ITask> eventArgs)
        {
            this.CurrentTask = null;
            this._eventAggregator.PublishOnUIThread(new TaskExecutedEvent(eventArgs.Item));

            this.Logger.ErrorFormat(eventArgs.Exception, string.Format("An exception occured in the task '{0}'.", eventArgs.Item.GetType().Name));

            if (eventArgs.Exception is TaskException)
            {
                this._displayManager.Messages.ShowMessageBox(eventArgs.Exception.Message, TaskMessages.ErrorInTask, MessageBoxButton.OK);
            }
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
