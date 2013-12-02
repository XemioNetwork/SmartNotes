using System;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Windows.Media.Imaging;
using Caliburn.Micro;
using Xemio.SmartNotes.Client.Shared.WebService;
using Xemio.SmartNotes.Client.Windows.Data.Events;
using Xemio.SmartNotes.Client.Windows.Implementations.Interaction;
using Xemio.SmartNotes.Client.Windows.Views.Content.AllNotes;
using Xemio.SmartNotes.Client.Windows.Views.Content.Search;

namespace Xemio.SmartNotes.Client.Windows.Views.Shell
{
    public class ShellViewModel : Screen
    {
        #region Fields
        private readonly DisplayManager _windowManager;
        private readonly WebServiceClient _webServiceClient;

        private readonly AllNotesViewModel _allNotesViewModel;
        private readonly SearchViewModel _searchViewModel;

        private Screen _currentContent;
        private BitmapImage _userAvatar;
        #endregion

        #region Properties
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
        /// <param name="displayManager">The display manager.</param>
        /// <param name="webServiceClient">The webservice client.</param>
        public ShellViewModel(DisplayManager displayManager, WebServiceClient webServiceClient)
        {
            this.DisplayName = "Xemio Notes";

            this._windowManager = displayManager;
            this._webServiceClient = webServiceClient;

            this._allNotesViewModel = IoC.Get<AllNotesViewModel>();
            this._searchViewModel = IoC.Get<SearchViewModel>();

            this.CurrentContent = this._allNotesViewModel;

            this.LoadUserAvatar();
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
            this.CurrentContent = this._allNotesViewModel;
        }
        /// <summary>
        /// Shows the search view.
        /// </summary>
        public void ShowSearch()
        {
            this.CurrentContent = this._searchViewModel;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Loads the user avatar.
        /// </summary>
        private async void LoadUserAvatar()
        {
            HttpResponseMessage response = await this._webServiceClient.Avatars.GetAvatar(40, 40);

            if (response.StatusCode == HttpStatusCode.Found)
            {
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.StreamSource = await response.Content.ReadAsStreamAsync();
                image.EndInit();

                this.UserAvatar = image.Clone();
            }
        }
        #endregion
    }
}
