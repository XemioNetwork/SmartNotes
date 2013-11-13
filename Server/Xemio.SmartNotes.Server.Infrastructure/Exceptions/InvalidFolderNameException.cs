using System;
using System.Net;
using System.Runtime.Serialization;
using Xemio.SmartNotes.Server.Infrastructure.Exceptions.Resources;

namespace Xemio.SmartNotes.Server.Infrastructure.Exceptions
{
    [Serializable]
    public class InvalidFolderNameException : BusinessException
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidFolderNameException"/> class.
        /// </summary>
        public InvalidFolderNameException()
            : base(ExceptionMessages.InvalidFolderName)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidFolderNameException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public InvalidFolderNameException(string message)
            : base(message)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidFolderNameException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        public InvalidFolderNameException(string message, Exception inner)
            : base(message, inner)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidFolderNameException"/> class.
        /// </summary>
        /// <param name="info">The info.</param>
        /// <param name="context">The context.</param>
        protected InvalidFolderNameException(SerializationInfo info, StreamingContext context)
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
