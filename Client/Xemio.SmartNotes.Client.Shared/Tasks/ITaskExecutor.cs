using System.Collections.Generic;

namespace Xemio.SmartNotes.Client.Shared.Tasks
{
    public interface ITaskExecutor
    {
        /// <summary>
        /// Gets the current task.
        /// </summary>
        ITask CurrentTask { get; }
        /// <summary>
        /// Gets the tasks.
        /// </summary>
        IReadOnlyCollection<ITask> Tasks { get; } 
        /// <summary>
        /// Starts the task.
        /// </summary>
        /// <param name="task">The task.</param>
        void StartTask(ITask task);
        /// <summary>
        /// Cancels the task. Returns whether it was canceled successfully.
        /// </summary>
        /// <param name="task">The task.</param>
        bool CancelTask(ITask task);
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
