namespace Xemio.SmartNotes.Client.Shared.Tasks
{
    public interface ITaskExecutor
    {
        /// <summary>
        /// Gets the current task.
        /// </summary>
        ITask CurrentTask { get; }
        /// <summary>
        /// Starts the task.
        /// </summary>
        /// <param name="task">The task.</param>
        void StartTask(ITask task);
        /// <summary>
        /// Determines whether this instance has tasks.
        /// </summary>
        bool HasTasks();
        /// <summary>
        /// Cancels the execution.
        /// </summary>
        void CancelExecution();
    }
}
