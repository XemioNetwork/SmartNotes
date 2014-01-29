using System;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using Caliburn.Micro;
using Castle.Core.Logging;
using Xemio.SmartNotes.Client.Abstractions.Tasks;
using Xemio.SmartNotes.Client.Shared.WebService;
using Xemio.SmartNotes.Client.Windows.Data.Events;
using Xemio.SmartNotes.Client.Windows.Implementations.Interaction;
using Xemio.SmartNotes.Client.Windows.Implementations.Tasks;
using Xemio.SmartNotes.Client.Windows.Views.Shell.AllNotes;
using Xemio.SmartNotes.Client.Windows.Views.Shell.Search;
using Xemio.SmartNotes.Client.Windows.Views.Shell.UserSettings;
using Xemio.SmartNotes.Models.Entities.Notes;

namespace Xemio.SmartNotes.Client.Windows.Views.Shell
{
    public class ShellViewModel : Conductor<Screen>, IHandle<LogoutEvent>, IHandle<ExecutingTaskEvent>, IHandle<ExecutedTaskEvent>, IHandleWithTask<AvatarChangedEvent>
    {
        #region Fields
        private readonly DisplayManager _displayManager;
        private readonly WebServiceClient _webServiceClient;
        private readonly IEventAggregator _eventAggregator;
        private readonly ITaskExecutor _taskExecutor;

        private readonly AllNotesViewModel _allNotesViewModel;
        private readonly SearchViewModel _searchViewModel;
        private readonly UserSettingsViewModel _userSettingsViewModel;

        private BitmapImage _userAvatar;
        private string _currentAction;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        public ILogger Logger { get; set; }
        /// <summary>
        /// Gets or sets the user avatar.
        /// </summary>
        public BitmapImage UserAvatar
        {
            get { return this._userAvatar; }
            set
            {
                if (this._userAvatar != value)
                {
                    this._userAvatar = value;
                    this.NotifyOfPropertyChange(() => this.UserAvatar);
                }
            }
        }
        /// <summary>
        /// Gets or sets the current executing action.
        /// </summary>
        public string CurrentAction
        {
            get { return this._currentAction; }
            set
            {
                if (this._currentAction != value)
                { 
                    this._currentAction = value;
                    this.NotifyOfPropertyChange(() => this.CurrentAction);
                }
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ShellViewModel"/> class.
        /// </summary>
        /// <param name="displayManager">The display manager.</param>
        /// <param name="webServiceClient">The webservice client.</param>
        /// <param name="eventAggregator">The event aggregator.</param>
        /// <param name="taskExecutor">The task executor.</param>
        public ShellViewModel(DisplayManager displayManager, WebServiceClient webServiceClient, IEventAggregator eventAggregator, ITaskExecutor taskExecutor)
        {
            this.Logger = NullLogger.Instance;
            this.DisplayName = "Xemio Notes";

            this._displayManager = displayManager;
            this._webServiceClient = webServiceClient;
            this._eventAggregator = eventAggregator;
            this._taskExecutor = taskExecutor;

            this._allNotesViewModel = IoC.Get<AllNotesViewModel>();
            this._searchViewModel = IoC.Get<SearchViewModel>();
            this._userSettingsViewModel = IoC.Get<UserSettingsViewModel>();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Opens the xemio website.
        /// </summary>
        public void OpenXemioWebsite()
        {
            Process.Start("http://www.xemio.net");
        }
        /// <summary>
        /// Shows all notes.
        /// </summary>
        public void ShowAllNotes()
        {
            this.ActivateItem(this._allNotesViewModel);
        }
        /// <summary>
        /// Shows the search view.
        /// </summary>
        public void ShowSearch()
        {
            this.ActivateItem(this._searchViewModel);
        }
        /// <summary>
        /// Shows the user settings.
        /// </summary>
        public void ShowUserSettings()
        {
            this.ActivateItem(this._userSettingsViewModel);
        }
        #endregion

        #region Overrides of Screen
        /// <summary>
        /// Called when initializing.
        /// </summary>
        protected override async void OnInitialize()
        {
            this._eventAggregator.Subscribe(this);
            await this.LoadUserAvatar();
            this.ActivateItem(this._allNotesViewModel);
        }

        #endregion

        #region Overrides of Conductor<Screen>
        /// <summary>
        /// Determines whether the view can be closed.
        /// </summary>
        /// <param name="callback">The callback.</param>
        public override void CanClose(Action<bool> callback)
        {
            if (this._taskExecutor.HasTasks() == false)
            {
                callback(true);
                return;
            }

            MessageBoxResult result = this._displayManager.Messages.ShowMessageBox("Es gibt noch Tasks die ausgeführt werden müssen, möchten Sie das Programm wirklich beenden?", "Beenden?", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            { 
                this._taskExecutor.CancelExecution();
                callback(true);
            }
            else
            {
                callback(false);
            }
        }
        #endregion

        #region Implementation of IHandle<LogoutEvent>
        /// <summary>
        /// Handles the <see cref="LogoutEvent"/>.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Handle(LogoutEvent message)
        {
            this.TryClose(true);
        }
        #endregion

        #region Implementation of IHandle<ExecutingTaskEvent>
        /// <summary>
        /// Handles the <see cref="ExecutingTaskEvent"/>.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Handle(ExecutingTaskEvent message)
        {
            this.CurrentAction = message.Task.DisplayName;
        }
        #endregion

        #region Implementation of IHandle<ExecutedTaskEvent>
        /// <summary>
        /// Handles the <see cref="ExecutedTaskEvent"/>
        /// </summary>
        /// <param name="message">The message.</param>
        public void Handle(ExecutedTaskEvent message)
        {
            this.CurrentAction = string.Empty;
        }
        #endregion

        #region Implementation of IHandleWithTask<AvatarChangedEvent>
        /// <summary>
        /// Handles the <see cref="AvatarChangedEvent"/>.
        /// </summary>
        /// <param name="message">The message.</param>
        public async Task Handle(AvatarChangedEvent message)
        {
            await this.LoadUserAvatar();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Loads the user avatar.
        /// </summary>
        private async Task LoadUserAvatar()
        {
            HttpResponseMessage response = await this._webServiceClient.Avatars.GetAvatar(40, 40);

            if (response.StatusCode == HttpStatusCode.Found)
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.StreamSource = await response.Content.ReadAsStreamAsync();
                image.EndInit();

                this.UserAvatar = image;
            }
            else
            {
                string message = await response.Content.ReadAsStringAsync();
                this._displayManager.Messages.ShowMessageBox(message, ClientMessages.Error, MessageBoxButton.OK, MessageBoxImage.Error);

                this.Logger.ErrorFormat("Error while loading avatar from user '{0}': {1}", this._webServiceClient.Session.User.Id, message);
            }
        }
        #endregion
    }
}