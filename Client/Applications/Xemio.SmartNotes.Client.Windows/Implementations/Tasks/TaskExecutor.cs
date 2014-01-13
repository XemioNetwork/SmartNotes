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
using Xemio.SmartNotes.Abstractions.Common;
using Xemio.SmartNotes.Client.Abstractions.Tasks;
using Xemio.SmartNotes.Client.Windows.Data.Events;
using Xemio.SmartNotes.Client.Windows.Data.Exceptions;
using Xemio.SmartNotes.Client.Windows.Implementations.Interaction;

namespace Xemio.SmartNotes.Client.Windows.Implementations.Tasks
{
    public class TaskExecutor : ITaskExecutor, IDisposable
    {
        #region Fields
        private readonly IEventAggregator _eventAggregator;
        private readonly DisplayManager _displayManager;

        private readonly BackgroundQueue<ITask> _taskQueue;
        #endregion

        #region Properties
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
            this._eventAggregator.Publish(new ExecutingTaskEvent(task));

            task.Execute().Wait();

            this._eventAggregator.Publish(new ExecutedTaskEvent(task));
        }
        /// <summary>
        /// Called when an exception happens while executing a task.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">The event arguments.</param>
        private void OnException(object sender, BackgroundExceptionEventArgs<ITask> eventArgs)
        {
            this._eventAggregator.Publish(new ExecutedTaskEvent(eventArgs.Item));

            this.Logger.ErrorFormat(eventArgs.Exception, string.Format("An exception occured in the task '{0}'.", eventArgs.Item.GetType().Name));

            if (eventArgs.Exception is GenericException)
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
