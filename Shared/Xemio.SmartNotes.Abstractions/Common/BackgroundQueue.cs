using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Xemio.SmartNotes.Abstractions.Common
{
    /// <summary>
    /// A queue executing a given action on all enqueued items.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BackgroundQueue<T> : IDisposable
    {
        #region Fields
        private readonly CancellationTokenSource _cancellationTokenSource;

        private readonly AutoResetEvent _autoResetEvent;
        private readonly ConcurrentQueue<T> _queue;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the action executed on each item.
        /// </summary>
        public Action<T> Execute { get; private set; }
        /// <summary>
        /// Gets the action executed when an exception happened.
        /// </summary>
        public Func<Exception, bool> OnException { get; private set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="BackgroundQueue{T}"/> class.
        /// </summary>
        /// <param name="execute">The execute.</param>
        public BackgroundQueue(Action<T> execute)
            : this()
        {
            this.Execute = execute;
            this.OnException = this.DefaultOnException;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="BackgroundQueue{T}" /> class.
        /// </summary>
        /// <param name="execute">The execute.</param>
        /// <param name="onException">The action executed when an exception happened.</param>
        public BackgroundQueue(Action<T> execute, Func<Exception, bool> onException)
            : this()
        {

            this.Execute = execute;
            this.OnException = onException;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="BackgroundQueue{T}"/> class.
        /// </summary>
        private BackgroundQueue()
        {
            this._queue = new ConcurrentQueue<T>();
            this._autoResetEvent = new AutoResetEvent(false);

            this._cancellationTokenSource = new CancellationTokenSource();

            Task.Factory.StartNew(this.ExecuteOnItems, this._cancellationTokenSource.Token);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Enqueues the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Enqueue(T item)
        {
            this._queue.Enqueue(item);

            this._autoResetEvent.Set();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Executes the on items.
        /// </summary>
        private void ExecuteOnItems()
        {
            while (this._cancellationTokenSource.IsCancellationRequested == false)
            {
                //We wait 50 ms for a "Set" of the AutoResetEvent
                //If we don't receive it we check again if the cancellation is requested
                //With this we can stop the task
                if (this._autoResetEvent.WaitOne(TimeSpan.FromMilliseconds(50)) == false)
                    continue;

                T item;
                while (this._queue.TryDequeue(out item))
                {
                    try
                    {
                        this.Execute(item);
                    }
                    catch (Exception exception)
                    {
                        if (this.OnException(exception) == false)
                        {
                            this._cancellationTokenSource.Cancel(false);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// The default method used for OnException.
        /// </summary>
        /// <param name="exception">The exception.</param>
        private bool DefaultOnException(Exception exception)
        {
            return true;
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
