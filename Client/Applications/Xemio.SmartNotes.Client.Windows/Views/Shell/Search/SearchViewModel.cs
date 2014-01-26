﻿using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using Castle.Core.Logging;
using Xemio.SmartNotes.Client.Shared.WebService;
using Xemio.SmartNotes.Client.Windows.Data.Events;
using Xemio.SmartNotes.Client.Windows.Implementations.Interaction;
using Xemio.SmartNotes.Models.Entities.Notes;

namespace Xemio.SmartNotes.Client.Windows.Views.Shell.Search
{
    public class SearchViewModel : Conductor<Screen>, IHandleWithTask<SuggestionSelectedEvent>
    {
        #region Fields
        private readonly WebServiceClient _client;
        private readonly DisplayManager _displayManager;
        private readonly IEventAggregator _eventAggregator;

        private string _searchText;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        public ILogger Logger { get; set; }
        /// <summary>
        /// Gets or sets the search text.
        /// </summary>
        public string SearchText
        {
            get { return this._searchText; }
            set
            {
                if (value != this._searchText)
                {
                    this._searchText = value;
                    this.NotifyOfPropertyChange(() => this.SearchText);
                }
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="SearchViewModel"/> class.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="displayManager">The display manager.</param>
        /// <param name="eventAggregator">The event aggregator.</param>
        public SearchViewModel(WebServiceClient client, DisplayManager displayManager, IEventAggregator eventAggregator)
        {
            this.Logger = NullLogger.Instance;

            this._client = client;
            this._displayManager = displayManager;
            this._eventAggregator = eventAggregator;

            this._eventAggregator.Subscribe(this);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Searches the <see cref="Note"/>s matching the current <see cref="SearchText"/>.
        /// </summary>
        public async Task Search()
        {
            HttpResponseMessage response = await this._client.Notes.SearchNotes(this.SearchText);

            switch (response.StatusCode)
            {
                case HttpStatusCode.Found:
                {
                    var foundNotesViewModel = IoC.Get<FoundNotesViewModel>();
                    foundNotesViewModel.FoundNotes = await response.Content.ReadAsAsync<BindableCollection<Note>>();

                    this.ActivateItem(foundNotesViewModel);
                    break;
                }
                case HttpStatusCode.SeeOther:
                {
                    var suggestionsViewModel = IoC.Get<SuggestionsViewModel>();
                    suggestionsViewModel.Suggestions = await response.Content.ReadAsAsync<BindableCollection<string>>();

                    this.ActivateItem(suggestionsViewModel);
                    break;
                }
                case HttpStatusCode.NotFound:
                {
                    var nothingFoundViewModel = IoC.Get<NothingFoundViewModel>();

                    this.ActivateItem(nothingFoundViewModel);
                    break;
                }
                default:
                {
                    string error = await response.Content.ReadAsStringAsync();
                    this._displayManager.Messages.ShowMessageBox(error, ClientMessages.Error, MessageBoxButton.OK, MessageBoxImage.Error);

                    this.Logger.ErrorFormat("Error while searching for notes with searchtext '{0}': {1}", this.SearchText, error);
                    break;
                }
            }
        }
        #endregion

        #region Overrides of Conductor<Screen>
        /// <summary>
        /// Called when deactivating.
        /// </summary>
        /// <param name="close">Inidicates whether this instance will be closed.</param>
        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);
            
            if (close)
                this._eventAggregator.Unsubscribe(this);
        }
        #endregion

        #region Implementation of IHandle<SuggestionSelectedEvent>
        /// <summary>
        /// Handles the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public async Task Handle(SuggestionSelectedEvent message)
        {
            this.SearchText = message.Suggestion;

            await this.Search();
        }
        #endregion
    }
}
