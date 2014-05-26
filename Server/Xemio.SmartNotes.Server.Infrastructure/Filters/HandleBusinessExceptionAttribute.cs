using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Filters;
using Castle.Core.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xemio.SmartNotes.Server.Infrastructure.Controllers;
using Xemio.SmartNotes.Server.Infrastructure.Exceptions;
using Xemio.SmartNotes.Server.Infrastructure.Extensions;
using Xemio.SmartNotes.Server.Infrastructure.Filters.Resources;
using Xemio.SmartNotes.Shared.Models;

namespace Xemio.SmartNotes.Server.Infrastructure.Filters
{
    /// <summary>
    /// Handles the <see cref="BusinessException"/>s.
    /// </summary>
    public class HandleBusinessExceptionAttribute : ExceptionFilterAttribute
    {
        #region Overrides of ExceptionFilterAttribute
        /// <summary>
        /// Called when an exception occured.
        /// </summary>
        /// <param name="context">The context.</param>
        public override void OnException(HttpActionExecutedContext context)
        {
            ILogger logger = this.GetLogger(context);
            logger.Error("Exception occured", context.Exception);

            //We need to communicate that an exception occured
            if (context.ActionContext.ControllerContext.Controller is BaseController)
            {
                var controller = (BaseController) context.ActionContext.ControllerContext.Controller;
                controller.ExceptionOccured = true;
            }

            HttpError error = this.GetError(context.Exception);
            HttpStatusCode status = this.GetStatusCode(context.Exception);

            context.Response = context.Request.CreateErrorResponse(status, error);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Returns a <see cref="ILogger"/> for the current <see cref="HttpActionExecutedContext"/>.
        /// </summary>
        /// <param name="context">The context.</param>
        private ILogger GetLogger(HttpActionExecutedContext context)
        {
            ILoggerFactory loggerFactory = context.ActionContext.ControllerContext.Configuration.DependencyResolver.GetService<ILoggerFactory>();
            string loggerName = context.ActionContext.ActionDescriptor.ControllerDescriptor.ControllerType.FullName;

            return loggerFactory.Create(loggerName);
        }
        /// <summary>
        /// Creates the <see cref="HttpError"/> for the specified <paramref name="exception"/>.
        /// </summary>
        /// <param name="exception">The exception.</param>
        private HttpError GetError(Exception exception)
        {
            if (exception is BusinessException)
            {
                var businessException = (BusinessException)exception;

                var error = new HttpError(FilterMessages.InternalServerError);
                error.Add("AdditionalData", businessException.CustomResponse);

                return error;
            }
            else
            {
#if DEBUG
                return new HttpError(exception.ToString());
#else
                return new HttpError(FilterMessages.InternalServerError);
#endif
            }
        }
        /// <summary>
        /// Creates the <see cref="HttpStatusCode"/> for the specified <paramref name="exception"/>.
        /// </summary>
        /// <param name="exception">The exception.</param>
        private HttpStatusCode GetStatusCode(Exception exception)
        {
            if (exception is BusinessException)
            {
                var businessException = (BusinessException) exception;
                return businessException.StatusCode;
            }
            else
            {
                return HttpStatusCode.InternalServerError;
            }
        }
        #endregion
    }
}
