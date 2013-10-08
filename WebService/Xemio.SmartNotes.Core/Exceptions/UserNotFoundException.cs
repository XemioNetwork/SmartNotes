using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Xemio.SmartNotes.Core.Exceptions.Resources;

namespace Xemio.SmartNotes.Core.Exceptions
{
    [Serializable]
    public class UserNotFoundException : BusinessException
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="UserNotFoundException" /> class.
        /// </summary>
        /// <param name="userId">The user id.</param>
        public UserNotFoundException(int userId)
            : this(string.Format(ExceptionMessages.UserNotFound, userId))
        {
            this.UserId = userId;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="UserNotFoundException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public UserNotFoundException(string message)
            : base(message)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="UserNotFoundException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        public UserNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="UserNotFoundException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        protected UserNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            this.UserId = info.GetInt32("UserId");
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the user id.
        /// </summary>
        public int UserId { get; private set; }
        #endregion

        #region Overrides of BusinessException
        /// <summary>
        /// Gets the status code of the webapi response.
        /// </summary>
        public override HttpStatusCode StatusCode
        {
            get { return HttpStatusCode.NotFound; }
        }
        #endregion

        #region Overrides of Exception
        /// <summary>
        /// When overridden in a derived class, sets the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> with information about the exception.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("UserId", this.UserId);
        }
        #endregion
    }
}
