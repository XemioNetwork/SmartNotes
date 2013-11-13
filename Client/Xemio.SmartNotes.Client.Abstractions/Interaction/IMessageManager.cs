using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Xemio.SmartNotes.Client.Abstractions.Interaction
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
