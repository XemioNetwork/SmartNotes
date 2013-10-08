using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using Xemio.SmartNotes.Core.Exceptions.Resources;

namespace Xemio.SmartNotes.Core.Exceptions
{
    [Serializable]
    public class InvalidNoteNameException : BusinessException
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidNoteNameException"/> class.
        /// </summary>
        public InvalidNoteNameException()
            : base(ExceptionMessages.InvalidNoteName)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidNoteNameException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public InvalidNoteNameException(string message)
            : base(message)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidNoteNameException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        public InvalidNoteNameException(string message, Exception inner)
            : base(message, inner)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidNoteNameException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        protected InvalidNoteNameException(SerializationInfo info, StreamingContext context)
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
