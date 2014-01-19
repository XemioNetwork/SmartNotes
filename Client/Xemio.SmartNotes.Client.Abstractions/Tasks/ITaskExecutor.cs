using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xemio.SmartNotes.Client.Abstractions.Tasks
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
