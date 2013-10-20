using System;
using Caliburn.Micro;
using Xemio.SmartNotes.Client.Data;
using Xemio.SmartNotes.Client.Data.Events;
using Xemio.SmartNotes.Client.UserInterface.Common;
using Xemio.SmartNotes.Client.UserInterface.Views.Header;
using Xemio.SmartNotes.Client.UserInterface.Views.Login;
using Xemio.SmartNotes.Entities.Users;

namespace Xemio.SmartNotes.Client.UserInterface.Views.Shell
{
    public class ShellViewModel : Screen, IHandle<ChangeContentEvent>
    {
        #region Fields
        private readonly IEventAggregator _eventAggregator;
        private readonly IWindowManager _windowManager;

        private HeaderViewModel _header;
        private Screen _currentContent;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the header.
        /// </summary>
        public HeaderViewModel Header
        {
            get { return this._header; }
            set
            {
                if (this._header != value)
                {
                    this._header = value;
                    this.NotifyOfPropertyChange(() => this.Header);
                }
            }
        }
        /// <summary>
        /// Gets or sets the current screen.
        /// </summary>
        public Screen CurrentContent
        {
            get { return this._currentContent; }
            set
            {
                if (this._currentContent != value)
                {
                    this._currentContent = value;
                    this.NotifyOfPropertyChange(() => this.CurrentContent);
                }
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ShellViewModel"/> class.
        /// </summary>
        /// <param name="eventAggregator">The event aggregator.</param>
        /// <param name="windowManager">The window manager.</param>
        /// <param name="headerViewModel">The header view model.</param>
        public ShellViewModel(IEventAggregator eventAggregator, IWindowManager windowManager, HeaderViewModel headerViewModel)
        {
            this.DisplayName = "Xemio - Smart Notes";

            this._eventAggregator = eventAggregator;
            this._eventAggregator.Subscribe(this);

            this._windowManager = windowManager;

            this.Header = headerViewModel;
            this.Header.ConductWith(this);
        }
        #endregion

        #region Implementation of IHandle<ChangeContentEvent>
        /// <summary>
        /// Handles the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Handle(ChangeContentEvent message)
        {
            this.CurrentContent = message.NextContent;
        }
        #endregion
    }
}
