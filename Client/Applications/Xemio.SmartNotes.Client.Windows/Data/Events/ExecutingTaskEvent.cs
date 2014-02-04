using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xemio.SmartNotes.Client.Shared.Tasks;

namespace Xemio.SmartNotes.Client.Windows.Data.Events
{
    /// <summary>
    /// Fired when the current <see cref="ITask"/> of the <see cref="ITaskExecutor"/> changed.
    /// </summary>
    public class ExecutingTaskEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExecutingTaskEvent"/> class.
        /// </summary>
        /// <param name="task">The new current task.</param>
        public ExecutingTaskEvent(ITask task)
        {
            this.Task = task;
        }

        /// <summary>
        /// Gets the new current task.
        /// </summary>
        public ITask Task { get; private set; }
    }
}
