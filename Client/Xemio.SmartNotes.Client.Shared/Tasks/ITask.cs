using System.Threading.Tasks;

namespace Xemio.SmartNotes.Client.Shared.Tasks
{
    public interface ITask
    {
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
