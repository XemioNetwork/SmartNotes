using System;
using System.Net;
using System.Runtime.Serialization;
using Xemio.SmartNotes.Server.Infrastructure.Exceptions.Resources;

namespace Xemio.SmartNotes.Server.Infrastructure.Exceptions
{
    [Serializable]
    public class UsernameUnavailableException : BusinessException
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="UsernameUnavailableException"/> class.
        /// </summary>
        public UsernameUnavailableException(string username)
            : base(string.Format(ExceptionMessages.UsernameUnavailable, username))
        {
            this.Username = username;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="UsernameUnavailableException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        public UsernameUnavailableException(string message, Exception inner)
            : base(message, inner)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="UsernameUnavailableException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        protected UsernameUnavailableException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            this.Username = info.GetString("Username");
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the username.
        /// </summary>
        public string Username { get; private set; }
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

        #region Overrides of Exception
        /// <summary>
        /// When overridden in a derived class, sets the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> with information about the exception.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("Username", this.Username);
        }
        #endregion
    }
}
