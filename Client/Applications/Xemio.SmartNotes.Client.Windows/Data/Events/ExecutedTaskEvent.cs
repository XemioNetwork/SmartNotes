using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xemio.SmartNotes.Client.Abstractions.Tasks;

namespace Xemio.SmartNotes.Client.Windows.Data.Events
{
    /// <summary>
    /// Fired when a <see cref="ITask"/> was executed.
    /// </summary>
    public class ExecutedTaskEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExecutedTaskEvent"/> class.
        /// </summary>
        /// <param name="task">The task.</param>
        public ExecutedTaskEvent(ITask task)
        {
            this.Task = task;
        }

        /// <summary>
        /// Gets the task which was executed.
        /// </summary>
        public ITask Task { get; private set; }
    }
}
