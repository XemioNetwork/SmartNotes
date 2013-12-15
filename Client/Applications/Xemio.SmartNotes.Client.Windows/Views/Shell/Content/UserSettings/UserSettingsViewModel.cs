using Caliburn.Micro;
using Xemio.SmartNotes.Client.Windows.Data.Events;

namespace Xemio.SmartNotes.Client.Windows.Views.Shell.Content.UserSettings
{
    public class UserSettingsViewModel : Screen
    {
        #region Fields
        private readonly IEventAggregator _eventAggregator;
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
        public void Logout()
        {
            this._eventAggregator.Publish(new LogoutEvent());
        }
        #endregion
    }
}
