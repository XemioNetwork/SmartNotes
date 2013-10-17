using System;
using System.Diagnostics;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Caliburn.Micro;
using Xemio.SmartNotes.Client.Data;
using Xemio.SmartNotes.Client.UserInterface.Images;
using Xemio.SmartNotes.Client.UserInterface.Views.Content.AllNotes;
using Xemio.SmartNotes.Client.UserInterface.Views.Content.Search;

namespace Xemio.SmartNotes.Client.UserInterface.Views.Header
{
    public class HeaderViewModel : Conductor<Screen>
    {
        #region Fields
        private readonly IEventAggregator _eventAggregator;
        private readonly Session _session;

        private readonly AllNotesViewModel _allNotesViewModel;
        private readonly SearchViewModel _searchViewModel;

        private BitmapImage _image;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the image.
        /// </summary>
        public BitmapImage Image
        {
            get { return this._image; }
            set
            {
                if (this._image != value)
                {
                    this._image = value;
                    this.NotifyOfPropertyChange(() => this.Image);
                }
            }
        }
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HeaderViewModel"/> class.
        /// </summary>
        /// <param name="eventAggregator">The event aggregator.</param>
        /// <param name="allNotesViewModel">All notes view model.</param>
        /// <param name="searchViewModel">The search view model.</param>
        /// <param name="session">The application session.</param>
        public HeaderViewModel(IEventAggregator eventAggregator, AllNotesViewModel allNotesViewModel, SearchViewModel searchViewModel, Session session)
        {
            this._eventAggregator = eventAggregator;
            this._session = session;

            this._allNotesViewModel = allNotesViewModel;
            this._searchViewModel = searchViewModel;

            this.Image = new BitmapImage(ImagePaths.DefaultUserIcon);
        }
        #endregion

        #region Overrides of Screen
        /// <summary>
        /// Called when this screen is initialized.
        /// </summary>
        protected override void OnInitialize()
        {
            this.ActivateItem(this._allNotesViewModel);
        }
        #endregion

        #region Overrides of IConductor
        /// <summary>
        /// Activates the item.
        /// </summary>
        /// <param name="item">The item.</param>
        public override void ActivateItem(Screen item)
        {
            base.ActivateItem(item);

            this._eventAggregator.Publish(new ChangeContentEvent(item));
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
        /// Shows the search view.
        /// </summary>
        public void ShowSearch()
        {
            this.ActivateItem(this._searchViewModel);
        }
        /// <summary>
        /// Shows all notes.
        /// </summary>
        public void ShowAllNotes()
        {
            this.ActivateItem(this._allNotesViewModel);
        }
        #endregion
    }
}
