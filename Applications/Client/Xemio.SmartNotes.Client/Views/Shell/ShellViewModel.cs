using Caliburn.Micro;
using Xemio.SmartNotes.Client.Events;
using Xemio.SmartNotes.Client.Views.Header;
using Xemio.SmartNotes.Client.Views.Login;

namespace Xemio.SmartNotes.Client.Views.Shell
{
    public class ShellViewModel : Screen, IHandle<ChangeCurrentScreenEvent>
    {
        #region Fields
        private readonly IEventAggregator _eventAggregator;
        private HeaderViewModel _header;
        private Screen _currentContent;
        private LoginViewModel _login;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ShellViewModel"/> class.
        /// </summary>
        /// <param name="eventAggregator">The event aggregator.</param>
        /// <param name="headerViewModel">The header view model.</param>
        /// <param name="loginViewModel">The login view model.</param>
        public ShellViewModel(IEventAggregator eventAggregator, HeaderViewModel headerViewModel, LoginViewModel loginViewModel)
        {
            this._eventAggregator = eventAggregator;
            this._eventAggregator.Subscribe(this);

            this.Login = loginViewModel;

            this.Header = headerViewModel;
            this.Header.ConductWith(this);
        }
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
        /// <summary>
        /// Gets or sets the login.
        /// </summary>
        public LoginViewModel Login
        {
            get { return this._login; }
            set
            {
                if (this._login != value)
                {
                    this._login = value;
                    this.NotifyOfPropertyChange(() => this.Login);
                }
            }
        }
        #endregion

        #region Implementation of IHandle<ChangeCurrentScreenEvent>
        /// <summary>
        /// Handles the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Handle(ChangeCurrentScreenEvent message)
        {
            this.CurrentContent = message.NextScreen;
        }
        #endregion
    }
}
