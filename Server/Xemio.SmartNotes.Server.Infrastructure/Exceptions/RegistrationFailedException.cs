using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xemio.SmartNotes.Server.Infrastructure.Exceptions.Resources;

namespace Xemio.SmartNotes.Server.Infrastructure.Exceptions
{
    public class RegistrationFailedException : BusinessException
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrationFailedException"/> class.
        /// </summary>
        public RegistrationFailedException()
            : base(ExceptionMessages.RegistrationFailed)
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
