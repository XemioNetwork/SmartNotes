using Xemio.SmartNotes.Client.Shared.Tasks;

namespace Xemio.SmartNotes.Client.Windows.Data.Events
{
    public abstract class TaskEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskEvent"/> class.
        /// </summary>
        /// <param name="task">The task.</param>
        protected TaskEvent(ITask task)
        {
            this.Task = task;
        }

        /// <summary>
        /// Gets the task.
        /// </summary>
        public ITask Task { get; private set; }
    }
}