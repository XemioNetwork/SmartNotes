using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xemio.SmartNotes.Client.Abstractions.Tasks;

namespace Xemio.SmartNotes.Client.Windows.Implementations.Tasks
{
    public class TaskExecutor : ITaskExecutor, IDisposable
    {
        #region Fields
        private readonly CancellationTokenSource _cancellationTokenSource;

        private readonly ConcurrentQueue<ITask> _taskQueue;

        private readonly AutoResetEvent _waitHandle;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskExecutor"/> class.
        /// </summary>
        public TaskExecutor()
        {
            this._taskQueue = new ConcurrentQueue<ITask>();
            this._waitHandle = new AutoResetEvent(false);

            this._cancellationTokenSource = new CancellationTokenSource();
            Task.Factory.StartNew(this.ExecuteTasks, this._cancellationTokenSource.Token);
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

            //Start the background-task
            this._waitHandle.Set();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Executes the tasks in the <see cref="_taskQueue"/>.
        /// </summary>
        private void ExecuteTasks()
        {
            while (this._cancellationTokenSource.IsCancellationRequested == false)
            {
                //We wait 100 milliseconds for a new task so we can cancel when requested
                if (this._waitHandle.WaitOne(TimeSpan.FromMilliseconds(100)) == false)
                    continue;

                ITask currentTask;
                //Execute all tasks we got in the queue
                while (this._taskQueue.TryDequeue(out currentTask))
                {
                    this.CurrentTask = currentTask;

                    currentTask.Execute().Wait();
                }
            }
        }
        #endregion

        #region Implementation of IDisposable
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this._cancellationTokenSource.Cancel();
        }
        #endregion
    }
}
