using System;
using System.Net;
using System.Runtime.Serialization;
using Xemio.SmartNotes.Server.Infrastructure.Exceptions.Resources;

namespace Xemio.SmartNotes.Server.Infrastructure.Exceptions
{
    [Serializable]
    public class InvalidUsernameException : BusinessException
    {
        #region Methods
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidUsernameException"/> class.
        /// </summary>
        public InvalidUsernameException()
            : base(ExceptionMessages.InvalidUsername)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidUsernameException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public InvalidUsernameException(string message)
            : base(message)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidUsernameException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        public InvalidUsernameException(string message, Exception inner)
            : base(message, inner)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidUsernameException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        protected InvalidUsernameException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        #endregion

        #region Overrides of BusinessException
        /// <summary>
        /// Gets the status code of the webapi response.
        /// </summary>
        public override HttpStatusCode StatusCode
        {
            get { return HttpStatusCode.BadRequest; }
        }
        #endregion
    }
}
