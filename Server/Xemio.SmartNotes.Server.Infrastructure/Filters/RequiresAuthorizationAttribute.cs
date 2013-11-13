using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using Raven.Client;
using Xemio.SmartNotes.Abstractions.Authorization;
using Xemio.SmartNotes.Models.Entities.Users;
using Xemio.SmartNotes.Server.Infrastructure.Extensions;
using Xemio.SmartNotes.Server.Infrastructure.Filters.Resources;
using Xemio.SmartNotes.Server.Infrastructure.Raven.Indexes;

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
                return false;

            if (this.AreRequiredDataPresent(context) == false)
                return false;

            if (this.UsernameExists(context) == false)
                return false;

            User user = this.GetUser(context);
            UserAuthentication data = this.GetAuthentication(context, user);

            string givenContentHash = this.GetContentHash(context);
            string computedContentHash = this.ComputeContentHash(context, data);

            if (givenContentHash != computedContentHash)
                return false;

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
            return context.Request.Headers.Authorization.Parameter.Split(':').Length == 2;
        }
        /// <summary>
        /// Determines whether the given username exists.
        /// </summary>
        /// <param name="context">The context.</param>
        private bool UsernameExists(HttpActionContext context)
        {
            string username = context.Request.Headers.Authorization.Parameter.Split(':').First();

            IAsyncDocumentSession documentSession = context.ControllerContext.Configuration.DependencyResolver.GetService<IAsyncDocumentSession>();
            User user = documentSession.Query<User, UsersByUsername>().FirstOrDefaultAsync(f => f.Username == username).Result;

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

            IAsyncDocumentSession documentSession = context.ControllerContext.Configuration.DependencyResolver.GetService<IAsyncDocumentSession>();
            return documentSession.Query<User, UsersByUsername>().FirstAsync(f => f.Username == username).Result;
        }
        /// <summary>
        /// Returns the <see cref="UserAuthentication"/> for the given <paramref name="user"/>.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="user">The user.</param>
        private UserAuthentication GetAuthentication(HttpActionContext context, User user)
        {
            IAsyncDocumentSession documentSession = context.ControllerContext.Configuration.DependencyResolver.GetService<IAsyncDocumentSession>();
            return documentSession.Query<UserAuthentication, UserAuthenticationsByUserId>().FirstAsync(f => f.UserId == user.Id).Result;
        }
        /// <summary>
        /// Computes the content hash using the given <paramref name="data"/>.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="data">The data.</param>
        private string ComputeContentHash(HttpActionContext context, UserAuthentication data)
        {
            string content = context.Request.Content.ReadAsStringAsync().Result;

            return AuthorizationHash.Create(data.AuthorizationHash, content);
        }
        #endregion
    }
}
