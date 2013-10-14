using System;
using System.Collections;
using System.Diagnostics;
using Caliburn.Micro;
using Xemio.SmartNotes.Client.Events;
using Xemio.SmartNotes.Client.Views.Content.AllNotes;
using Xemio.SmartNotes.Client.Views.Content.Search;
using Xemio.SmartNotes.Client.Views.Login;

namespace Xemio.SmartNotes.Client.Views.Header
{
    public class HeaderViewModel : Conductor<Screen>
    {
        #region Fields
        private readonly IEventAggregator _eventAggregator;
        private readonly AllNotesViewModel _allNotesViewModel;
        private readonly SearchViewModel _searchViewModel;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="HeaderViewModel"/> class.
        /// </summary>
        /// <param name="eventAggregator">The event aggregator.</param>
        /// <param name="allNotesViewModel">All notes view model.</param>
        /// <param name="searchViewModel">The search view model.</param>
        public HeaderViewModel(IEventAggregator eventAggregator, AllNotesViewModel allNotesViewModel, SearchViewModel searchViewModel)
        {
            this._eventAggregator = eventAggregator;
            this._allNotesViewModel = allNotesViewModel;
            this._searchViewModel = searchViewModel;
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

            this._eventAggregator.Publish(new ChangeCurrentScreenEvent(item));
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
