using System;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace Xemio.SmartNotes.Client.Shared.Tasks
{
    public interface ITask
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        Guid Id { get; }
        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        DateTimeOffset StartDate { get; set; }
        /// <summary>
        /// Gets the display name for this task.
        /// </summary>
        string DisplayName { get; }

        /// <summary>
        /// Executes this task.
        /// </summary>
        Task Execute();
    }
}
