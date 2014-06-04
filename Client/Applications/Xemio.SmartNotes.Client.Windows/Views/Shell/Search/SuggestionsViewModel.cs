using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Xemio.SmartNotes.Client.Windows.Data.Events;

namespace Xemio.SmartNotes.Client.Windows.Views.Shell.Search
{
    public class SuggestionsViewModel : Screen
    {
        #region Fields
        private readonly IEventAggregator _eventAggregator;

        private BindableCollection<string> _suggestions;
        #endregion
        
        #region Properties
        /// <summary>
        /// Gets or sets the suggestions.
        /// </summary>
        public BindableCollection<string> Suggestions
        {
            get { return this._suggestions; }
            set
            {
                if (this._suggestions != value)
                {
                    this._suggestions = value;
                    this.NotifyOfPropertyChange(() => this.Suggestions);
                }
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="SuggestionsViewModel"/> class.
        /// </summary>
        /// <param name="eventAggregator">The event aggregator.</param>
        public SuggestionsViewModel(IEventAggregator eventAggregator)
        {
            this._eventAggregator = eventAggregator;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Selects the suggestion.
        /// </summary>
        /// <param name="suggestion">The suggestion.</param>
        public void SelectSuggestion(string suggestion)
        {
            this._eventAggregator.PublishOnUIThread(new SuggestionSelectedEvent(suggestion));
        }
        #endregion
    }
}
