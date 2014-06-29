using Caliburn.Micro;
using Xemio.SmartNotes.Client.Shared.Tasks;
using Xemio.SmartNotes.Client.Windows.Data.Events;

namespace Xemio.SmartNotes.Client.Windows.Views.TaskList
{
    public class TaskListViewModel : Screen, IHandle<TaskEvent>
    {
        #region Fields
        private readonly ITaskExecutor _taskExecutor;

        private BindableCollection<ITask> _tasks;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the tasks.
        /// </summary>
        public BindableCollection<ITask> Tasks
        {
            get { return this._tasks; }
            set
            {
                if (this._tasks != value)
                {
                    this._tasks = value;
                    this.NotifyOfPropertyChange(() => this.Tasks);
                }
            }
        } 
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskListViewModel"/> class.
        /// </summary>
        /// <param name="taskExecutor">The task executor.</param>
        public TaskListViewModel(ITaskExecutor taskExecutor)
        {
            this._taskExecutor = taskExecutor;
        }
        #endregion

        #region Overrides of Screen
        /// <summary>
        /// Called when activating.
        /// </summary>
        protected override void OnActivate()
        {
            this.LoadTaskList();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Loads the task list.
        /// </summary>
        private void LoadTaskList()
        {
            this.Tasks = new BindableCollection<ITask>(this._taskExecutor.Tasks);
        }
        #endregion

        #region Implementation of IHandle<TaskEvent>
        /// <summary>
        /// Handles the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Handle(TaskEvent message)
        {
            this.LoadTaskList();
        }
        #endregion

    }
}