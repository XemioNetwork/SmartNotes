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
using Raven.Client;
using Xemio.SmartNotes.Server.Infrastructure.Extensions;
using Xemio.SmartNotes.Server.Infrastructure.Filters.Resources;
using Xemio.SmartNotes.Shared.Authorization;
using Xemio.SmartNotes.Shared.Entities.Users;

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
            actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized)
            {
                Content = new StringContent(FilterMessages.Unauthorized)
            };
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

            if (this.AreRequiredDataPresent(context) == false)
            {
                this.LogMessage(context, "Login failed. Authorization data missing.");
                return false;
            }

            if (this.UsernameExists(context) == false)
            {
                this.LogMessage(context, "Login failed. Username does not exist.");
                return false;   
            }

            User user = this.GetUser(context);

            string givenContentHash = this.GetContentHash(context);
            string computedContentHash = this.ComputeContentHash(context, user);

            if (givenContentHash != computedContentHash)
            {
                this.LogMessage(context, "Login failed. The calculated hash doesn't match the given one.");
                return false;
            }

            if (this.IsRequestDateValid(context) == false)
            {
                this.LogMessage(context, "Login failed. The request timed out.");
                return false;
            }

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(user.Id), new string[0]);
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
        /// Determines whether all data for xemio-authentication are present.
        /// </summary>
        /// <param name="context">The context.</param>
        private bool AreRequiredDataPresent(HttpActionContext context)
        {
            IEnumerable<string> requestDateValues;

            return context.Request.Headers.Authorization.Parameter.Split(':').Length == 2 &&
                   context.Request.Headers.TryGetValues("Request-Date", out requestDateValues);
        }
        /// <summary>
        /// Determines whether the given username exists.
        /// </summary>
        /// <param name="context">The context.</param>
        private bool UsernameExists(HttpActionContext context)
        {
            string username = context.Request.Headers.Authorization.Parameter.Split(':').First();

            var documentSession = context.ControllerContext.Configuration.DependencyResolver.GetService<IDocumentSession>();
            User user = documentSession.Query<User>().FirstOrDefault(f => f.Username == username);

            return user != null;
        }
        /// <summary>
        /// Returns the content-hash from the request.
        /// </summary>
        /// <param name="context">The context.</param>
        private string GetContentHash(HttpActionContext context)
        {
            return context.Request.Headers.Authorization.Parameter.Split(':').Last();
        }
        /// <summary>
        /// Returns the <see cref="User"/> executing the current request.
        /// </summary>
        /// <param name="context">The context.</param>
        private User GetUser(HttpActionContext context)
        {
            string username = context.Request.Headers.Authorization.Parameter.Split(':').First();

            var documentSession = context.ControllerContext.Configuration.DependencyResolver.GetService<IDocumentSession>();
            return documentSession.Query<User>().First(f => f.Username == username);
        }
        /// <summary>
        /// Computes the content hash using the given <paramref name="user"/>.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="user">The user.</param>
        private string ComputeContentHash(HttpActionContext context, User user)
        {
            string content = context.Request.Content.ReadAsStringAsync().Result;
            DateTimeOffset requestDate = DateTimeOffset.Parse(context.Request.Headers.GetValues("Request-Date").First());

            return AuthorizationHash.Create(user.AuthorizationHash, requestDate, content);
        }
        /// <summary>
        /// Determines whether the request date is valid.
        /// </summary>
        /// <param name="context">The context.</param>
        private bool IsRequestDateValid(HttpActionContext context)
        {
            DateTimeOffset requestDate = DateTimeOffset.Parse(context.Request.Headers.GetValues("Request-Date").First());
            var invalidDate = requestDate.AddMinutes(1);
            return invalidDate >= DateTimeOffset.Now;
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
