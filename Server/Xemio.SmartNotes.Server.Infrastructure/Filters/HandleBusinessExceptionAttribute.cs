using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using Castle.Core.Logging;
using Xemio.SmartNotes.Server.Infrastructure.Controllers;
using Xemio.SmartNotes.Server.Infrastructure.Exceptions;
using Xemio.SmartNotes.Server.Infrastructure.Extensions;
using Xemio.SmartNotes.Server.Infrastructure.Filters.Resources;

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
            
            if (context.Exception is BusinessException)
            {
                var businessException = (BusinessException)context.Exception;

                context.Response = new HttpResponseMessage(businessException.StatusCode)
                {
                    Content = new StringContent(businessException.Message)
                };
            }
            else
            {
#if DEBUG
                context.Response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                                       {
                                           Content = new StringContent(context.Exception.ToString())
                                       };
#else
                context.Response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(FilterMessages.InternalServerError)
                };
#endif
            }
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
        #endregion
    }
}
