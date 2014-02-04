using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Xemio.SmartNotes.Client.Shared.Interaction;
using Xemio.SmartNotes.Client.Shared.Settings;

namespace Xemio.SmartNotes.Client.Windows.Implementations.Interaction
{
    public class DisplayManager
    {
        #region Properties
        /// <summary>
        /// Gets the <see cref="IWindowManager"/>.
        /// </summary>
        public IWindowManager Windows { get; private set; }
        /// <summary>
        /// Gets the <see cref="IMessageManager"/>.
        /// </summary>
        public IMessageManager Messages { get; private set; }
        /// <summary>
        /// Gets the <see cref="ILanguageManager"/>.
        /// </summary>
        public ILanguageManager Languages { get; private set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DisplayManager"/> class.
        /// </summary>
        /// <param name="windows">The windows.</param>
        /// <param name="messages">The messages.</param>
        /// <param name="languages">The languages.</param>
        public DisplayManager(IWindowManager windows, IMessageManager messages, ILanguageManager languages)
        {
            this.Windows = windows;
            this.Messages = messages;
            this.Languages = languages;
        }
        #endregion
    }
}
