using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xemio.SmartNotes.Client.Windows.Data.Events
{
    /// <summary>
    /// Fired when the user selects a suggestion.
    /// </summary>
    public class SuggestionSelectedEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SuggestionSelectedEvent"/> class.
        /// </summary>
        /// <param name="suggestion">The suggestion.</param>
        public SuggestionSelectedEvent(string suggestion)
        {
            this.Suggestion = suggestion;
        }

        /// <summary>
        /// Gets the suggestion.
        /// </summary>
        public string Suggestion { get; private set; }
    }
}
