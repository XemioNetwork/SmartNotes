using Caliburn.Micro;
using Xemio.SmartNotes.Client.Windows.Data.Events;
using Xemio.SmartNotes.Client.Windows.Views.Shell.UserSettings.ChangeAvatar;

namespace Xemio.SmartNotes.Client.Windows.Views.Shell.UserSettings
{
    public class UserSettingsViewModel : Screen
    {
        #region Fields
        private readonly IEventAggregator _eventAggregator;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the change avatar.
        /// </summary>
        public ChangeAvatarViewModel ChangeAvatar { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="UserSettingsViewModel"/> class.
        /// </summary>
        /// <param name="eventAggregator">The event aggregator.</param>
        public UserSettingsViewModel(IEventAggregator eventAggregator)
        {
            this._eventAggregator = eventAggregator;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Logs the user out of the application.
        /// </summary>
        public void Logout()
        {
            this._eventAggregator.Publish(new LogoutEvent());
        }
        #endregion
    }
}
