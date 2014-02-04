using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Xemio.SmartNotes.Client.Shared.Interaction;

namespace Xemio.SmartNotes.Client.Windows.Implementations.Interaction
{
    public class MessageManager : IMessageManager
    {
        #region Implementation of IMessageManager
        /// <summary>
        /// Shows a message box.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="title">The title.</param>
        /// <param name="buttons">The buttons.</param>
        /// <param name="image">The image.</param>
        public MessageBoxResult ShowMessageBox(string message, string title, MessageBoxButton buttons, MessageBoxImage image = MessageBoxImage.None)
        {
            return MessageBox.Show(message, title, buttons, image);
        }
        #endregion
    }
}
