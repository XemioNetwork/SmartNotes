using System;

namespace Xemio.SmartNotes.Shared.Common
{
    public class BackgroundExceptionEventArgs<T> : EventArgs
    {
        public BackgroundExceptionEventArgs(T item, Exception exception)
        {
            this.Item = item;
            this.Exception = exception;

            this.CancelQueue = false;
        }

        public T Item { get; private set; }
        public Exception Exception { get; private set; }

        public bool CancelQueue { get; set; }
    }
}
