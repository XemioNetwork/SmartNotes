using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Windows;
using Caliburn.Micro;
using Castle.Core.Logging;
using Xemio.SmartNotes.Client.Shared.Clients;
using Xemio.SmartNotes.Client.Windows.Data.Events;
using Xemio.SmartNotes.Client.Windows.Implementations.Interaction;
using Xemio.SmartNotes.Client.Windows.ViewParts;
using Xemio.SmartNotes.Shared.Entities.Notes;
using Xemio.SmartNotes.Shared.Models;

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
                    var notes = await response.Content.ReadAsAsync<Note[]>();

                    var foundNotesViewModel = IoC.Get<FoundNotesViewModel>();
                    foundNotesViewModel.FoundNotes = new BindableCollection<NoteViewModel>(notes.Select(note =>
                    {
                        var viewModel = IoC.Get<NoteViewModel>();
                        viewModel.Initialize(note);

                        return viewModel;
                    }));

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
                    var error = await response.Content.ReadAsAsync<HttpError>();
                    this._displayManager.Messages.ShowMessageBox(error.Message, ClientMessages.Error, MessageBoxButton.OK, MessageBoxImage.Error);

                    this.Logger.ErrorFormat("Error while searching for notes with searchtext '{0}': {1}", this.SearchText, error.Message);
                    break;
                }
            }
        }
        #endregion
        
        #region Overrides of Screen
        /// <summary>
        /// Called when initializing.
        /// </summary>
        protected override void OnInitialize()
        {
            this._eventAggregator.Subscribe(this);
        }
        #endregion

        #region Implementation of IHandleWithTask<SuggestionSelectedEvent>
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
