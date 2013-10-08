using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using NLog;
using Xemio.SmartNotes.Core.Exceptions;
using Xemio.SmartNotes.Infrastructure.Extensions;

namespace Xemio.SmartNotes.Infrastructure.Filters
{
    /// <summary>
    /// Handles the <see cref="BusinessException"/>s.
    /// </summary>
    public class BusinessExceptionFilterAttribute : ExceptionFilterAttribute
    {
        /// <summary>
        /// Called when an exception occured.
        /// </summary>
        /// <param name="context">The context.</param>
        public override void OnException(HttpActionExecutedContext context)
        {
            Logger logger = this.GetLogger(context);
            logger.LogException(LogLevel.Error, "Exception occured", context.Exception);

            if (context.Exception is BusinessException)
            {
                var businessException = (BusinessException)context.Exception;

                context.Response = new HttpResponseMessage(businessException.StatusCode)
                                       {
                                           Content = new StringContent(businessException.Message)
                                       };
            }
        }

        /// <summary>
        /// Returns a <see cref="Logger"/> for the current <see cref="HttpActionExecutedContext"/>.
        /// </summary>
        /// <param name="context">The context.</param>
        private Logger GetLogger(HttpActionExecutedContext context)
        {
            LogFactory factory = context.ActionContext.ControllerContext.Configuration.DependencyResolver.GetService<LogFactory>();
            string loggerName = context.ActionContext.ActionDescriptor.ControllerDescriptor.ControllerType.FullName;

            return factory.GetLogger(loggerName);
        }
    }
}
