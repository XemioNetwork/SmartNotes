using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Xemio.SmartNotes.Client.Shared.Tasks;

namespace Xemio.SmartNotes.Client.Windows.Views.TaskList
{
    public class TaskViewModel : PropertyChangedBase
    {
        #region Fields
        private string _displayName;
        private DateTimeOffset _startDate;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the task.
        /// </summary>
        public ITask Task { get; private set; }


        public Guid Id { get; private set; }
        public string DisplayName
        {
            get { return this._displayName; }
            private set
            {
                if (this._displayName != value)
                { 
                    this._displayName = value;
                    this.NotifyOfPropertyChange(() => this.DisplayName);
                }
            }
        }

        public DateTimeOffset StartDate
        {
            get { return this._startDate; }
            private set
            {
                if (this._startDate != value)
                {
                    this._startDate = value;
                    this.NotifyOfPropertyChange(() => this.StartDate);
                }
            }
        }
        #endregion

        #region Methods
        public void Initialize(ITask task)
        {
            this.Task = task;

            this.Id = task.Id;
            this.DisplayName = task.DisplayName;
            this.StartDate = task.StartDate;
        }
        #endregion
    }
}
