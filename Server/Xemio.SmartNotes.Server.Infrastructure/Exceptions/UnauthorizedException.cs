using System;
using System.Net;
using System.Runtime.Serialization;
using Xemio.SmartNotes.Server.Infrastructure.Exceptions.Resources;

namespace Xemio.SmartNotes.Server.Infrastructure.Exceptions
{
    [Serializable]
    public class UnauthorizedException : BusinessException
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="UnauthorizedException"/> class.
        /// </summary>
        public UnauthorizedException()
            : base(ExceptionMessages.Unauthorized)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="UnauthorizedException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public UnauthorizedException(string message)
            : base(message)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="UnauthorizedException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        public UnauthorizedException(string message, Exception inner)
            : base(message, inner)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="UnauthorizedException"/> class.
        /// </summary>
        /// <param name="info">The info.</param>
        /// <param name="context">The context.</param>
        protected UnauthorizedException(SerializationInfo info, StreamingContext context)
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
            get { return HttpStatusCode.Forbidden; }
        }
        #endregion
    }
}
