using System;
using System.Net;
using System.Runtime.Serialization;
using Xemio.SmartNotes.Server.Infrastructure.Exceptions.Resources;

namespace Xemio.SmartNotes.Server.Infrastructure.Exceptions
{
    [Serializable]
    public class InvalidEmailAddressException : BusinessException
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidEmailAddressException"/> class.
        /// </summary>
        public InvalidEmailAddressException(string emailAddress)
            : base(string.Format(ExceptionMessages.InvalidEmailAddress, emailAddress))
        {
            this.EmailAddress = emailAddress;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidEmailAddressException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        public InvalidEmailAddressException(string message, Exception inner)
            : base(message, inner)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidEmailAddressException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        protected InvalidEmailAddressException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            this.EmailAddress = info.GetString("EmailAddress");
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the email address.
        /// </summary>
        public string EmailAddress { get; private set; }
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

            info.AddValue("EmailAddress", this.EmailAddress);
        }
        #endregion
    }
}
