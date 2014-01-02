using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http;
using System.Windows;
using Caliburn.Micro;
using Xemio.SmartNotes.Client.Shared.WebService;
using Xemio.SmartNotes.Models.Entities.Notes;

namespace Xemio.SmartNotes.Client.Windows.Views.Shell.Search
{
    public class SearchViewModel : Screen
    {
        #region Fields
        private readonly WebServiceClient _client;

        private string _searchText;
        private ObservableCollection<string> _foundNotes;
        #endregion

        #region Properties
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

        public ObservableCollection<string> FoundNotes
        {
            get { return this._foundNotes; }
            set
            {
                if (value != this._foundNotes)
                {
                    this._foundNotes = value;
                    this.NotifyOfPropertyChange(() => this.FoundNotes);
                }
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="SearchViewModel"/> class.
        /// </summary>
        /// <param name="client">The client.</param>
        public SearchViewModel(WebServiceClient client)
        {
            this._client = client;

            this.FoundNotes = new ObservableCollection<string>();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Searches the <see cref="Note"/>s matching the current <see cref="SearchText"/>.
        /// </summary>
        public async void Search()
        {
            HttpResponseMessage response = await this._client.Notes.GetAllNotes(this.SearchText);
            if (response.StatusCode == HttpStatusCode.Found)
            {
                this.FoundNotes.Clear();

                Note[] notes = await response.Content.ReadAsAsync<Note[]>();
                foreach (Note note in notes)
                {
                    this.FoundNotes.Add(string.Format("{0}, {1}", note.Name, note.Content));
                }
            }
            else
            {
                string error = await response.Content.ReadAsStringAsync();
                MessageBox.Show(string.Format("Something went wrong: {0}{1}", Environment.NewLine, error));
            }
        }
        #endregion
    }
}
