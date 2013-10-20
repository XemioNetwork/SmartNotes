using System.Windows;
using Caliburn.Micro;

namespace Xemio.SmartNotes.Client.UserInterface.Common
{
    public interface IMessageManager
    {
        /// <summary>
        /// Shows a message box.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="title">The title.</param>
        /// <param name="buttons">The buttons.</param>
        /// <param name="image">The image.</param>
        MessageBoxResult ShowMessageBox(string message, string title, MessageBoxButton buttons, MessageBoxImage image = MessageBoxImage.None);
    }
}
