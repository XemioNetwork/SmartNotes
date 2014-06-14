using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Logging;
using Xemio.SmartNotes.Client.Shared.Tasks;
using Xemio.SmartNotes.Client.Windows.Data.Exceptions;

namespace Xemio.SmartNotes.Client.Windows.Implementations.Tasks
{
    /// <summary>
    /// A base class for the <see cref="ITask"/> interface providing a <see cref="ILogger"/>.
    /// Tasks should throw in property setters to avoid wrong values.
    /// If something goes wrong while executing the task, log and throw an <see cref="GenericException"/>.
    /// </summary>
    public abstract class BaseTask : ITask
    {
        #region Properties
        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        public ILogger Logger { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseTask"/> class.
        /// </summary>
        protected BaseTask()
        {
            this.Logger = NullLogger.Instance;

            this.Id = Guid.NewGuid();
        }
        #endregion

        #region Implementation of ITask
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public Guid Id { get; private set; }
        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        public DateTimeOffset StartDate { get; set; }
        /// <summary>
        /// Gets the display name for this task.
        /// </summary>
        public abstract string DisplayName { get; }

        /// <summary>
        /// Executes this task.
        /// </summary>
        public abstract Task Execute();
        #endregion
    }
}
