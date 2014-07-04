using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Remoting.Contexts;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using Castle.Core.Logging;
using Newtonsoft.Json;
using Raven.Client;
using Xemio.SmartNotes.Server.Infrastructure.Extensions;
using Xemio.SmartNotes.Server.Infrastructure.Filters.Resources;
using Xemio.SmartNotes.Server.Infrastructure.RavenDB.Indexes;
using Xemio.SmartNotes.Shared.Entities.Users;
using Xemio.SmartNotes.Shared.Models;

namespace Xemio.SmartNotes.Server.Infrastructure.Filters
{
    /// <summary>
    /// Contains logic that authenticates a request.
    /// </summary>
    public class RequiresAuthorizationAttribute : AuthorizeAttribute
    {
        #region Overrides of AuthorizeAttribute
        /// <summary>
        /// Processes requests that fail authorization.
        /// </summary>
        /// <param name="actionContext">The context.</param>
        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, FilterMessages.Unauthorized);
        }
        /// <summary>
        /// Determines whether the specified context is authorized.
        /// </summary>
        /// <param name="context">The context.</param>
        protected override bool IsAuthorized(HttpActionContext context)
        {
            if (this.IsXemioAuthorization(context) == false)
            { 
                this.LogMessage(context, "Login failed. No 'Xemio' authorization");
                return false;
            }

            if (this.TokenExists(context) == false)
            {
                this.LogMessage(context, "Login failed. The token does not exist.");
                return false;   
            }
            
            AuthenticationToken token = this.GetToken(context);

            if (token.IsValid() == false)
            {
                this.LogMessage(context, "Login failed. The token has expired.");
                return false;
            }
            
            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(token.UserId), new string[0]);
            return true;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Determines whether the current request is using xemio-authentication.
        /// </summary>
        private bool IsXemioAuthorization(HttpActionContext context)
        {
            if (context.Request.Headers.Authorization == null)
                return false;

            return context.Request.Headers.Authorization.Scheme == "Xemio";
        }
        /// <summary>
        /// Determines whether the given token exists.
        /// </summary>
        /// <param name="context">The context.</param>
        private bool TokenExists(HttpActionContext context)
        {
            var token = this.GetToken(context);
            return token != null;
        }
        /// <summary>
        /// Returns the <see cref="AuthenticationToken"/> from the current request.
        /// </summary>
        /// <param name="context">The context.</param>
        private AuthenticationToken GetToken(HttpActionContext context)
        {
            string tokenString = context.Request.Headers.Authorization.Parameter;

            var documentSession = context.ControllerContext.Configuration.DependencyResolver.GetService<IDocumentSession>();

            var id = documentSession.Advanced.GetStringIdFor<AuthenticationToken>(tokenString);
            return documentSession.Load<AuthenticationToken>(id);
        }
        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="message">The message.</param>
        private void LogMessage(HttpActionContext context, string message)
        {
            var loggerFactory = context.ControllerContext.Configuration.DependencyResolver.GetService<ILoggerFactory>();
            ILogger logger = loggerFactory.Create(this.GetType());
            logger.Debug(message);
        }
        #endregion
    }
}
