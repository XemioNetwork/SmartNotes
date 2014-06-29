using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xemio.SmartNotes.Client.Shared.Tasks;

namespace Xemio.SmartNotes.Client.Windows.Data.Events
{
    /// <summary>
    /// Fired when an <see cref="ITask"/> was started.
    /// </summary>
    public class TaskStartedEvent : TaskEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskStartedEvent"/> class.
        /// </summary>
        /// <param name="task">The task.</param>
        public TaskStartedEvent(ITask task)
            : base(task)
        {
        }
    }
}
