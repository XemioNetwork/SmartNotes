using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xemio.SmartNotes.Client.Abstractions.Tasks;

namespace Xemio.SmartNotes.Client.Windows.Data.Events
{
    /// <summary>
    /// Fired when the current <see cref="ITask"/> of the <see cref="ITaskExecutor"/> changed.
    /// </summary>
    public class CurrentTaskChangedEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CurrentTaskChangedEvent"/> class.
        /// </summary>
        /// <param name="newCurrentTask">The new current task.</param>
        public CurrentTaskChangedEvent(ITask newCurrentTask)
        {
            this.NewCurrentTask = newCurrentTask;
        }

        /// <summary>
        /// Gets the new current task.
        /// </summary>
        public ITask NewCurrentTask { get; private set; }
    }
}
