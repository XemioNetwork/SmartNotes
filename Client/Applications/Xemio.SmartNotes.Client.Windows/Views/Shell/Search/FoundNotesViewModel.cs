using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Xemio.SmartNotes.Shared.Entities.Notes;

namespace Xemio.SmartNotes.Client.Windows.Views.Shell.Search
{
    public class FoundNotesViewModel : Screen
    {
        #region Fields
        private readonly IEventAggregator _eventAggregator;

        private BindableCollection<Note> _foundNotes;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="FoundNotesViewModel"/> class.
        /// </summary>
        /// <param name="eventAggregator">The event aggregator.</param>
        public FoundNotesViewModel(IEventAggregator eventAggregator)
        {
            this._eventAggregator = eventAggregator;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the found notes.
        /// </summary>
        public BindableCollection<Note> FoundNotes
        {
            get { return this._foundNotes; }
            set
            {
                if (this._foundNotes != value)
                { 
                    this._foundNotes = value;
                    this.NotifyOfPropertyChange(() => this.FoundNotes);
                }
            }
        }
        #endregion
    }
}
