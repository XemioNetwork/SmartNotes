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
    public class TaskExecutingEvent : TaskEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskExecutingEvent"/> class.
        /// </summary>
        /// <param name="task">The new current task.</param>
        public TaskExecutingEvent(ITask task)
            : base(task)
        {
        }
    }
}
