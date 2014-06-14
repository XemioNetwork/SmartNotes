using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xemio.SmartNotes.Client.Shared.Tasks;

namespace Xemio.SmartNotes.Client.Windows.Data.Events
{
    /// <summary>
    /// Fired when a <see cref="ITask"/> was executed.
    /// </summary>
    public class TaskExecutedEvent : TaskEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskExecutedEvent"/> class.
        /// </summary>
        /// <param name="task">The task.</param>
        public TaskExecutedEvent(ITask task)
            : base(task)
        {
        }
    }
}
