using Xemio.SmartNotes.Client.Shared.Tasks;

namespace Xemio.SmartNotes.Client.Windows.Data.Events
{
    /// <summary>
    /// Fired when a <see cref="ITask"/> was canceled.
    /// </summary>
    public class TaskCanceledEvent : TaskEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskCanceledEvent"/> class.
        /// </summary>
        /// <param name="task">The task.</param>
        public TaskCanceledEvent(ITask task) 
            : base(task)
        {
        }
    }
}