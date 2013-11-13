using Caliburn.Micro;

namespace Xemio.SmartNotes.Client.Windows.Data.Events
{
    /// <summary>
    /// Fired when the shell-content should be changed.
    /// </summary>
    public class ChangeContentEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeContentEvent"/> class.
        /// </summary>
        /// <param name="nextScreen">The next screen.</param>
        public ChangeContentEvent(Screen nextScreen)
        {
            this.NextContent = nextScreen;
        }

        /// <summary>
        /// Gets the next shell content.
        /// </summary>
        public Screen NextContent { get; private set; }
    }
}
