using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Controllers;
using Raven.Client;
using Xemio.SmartNotes.Entities.Users;
using Xemio.SmartNotes.Infrastructure.Raven.Indexes;
using Xemio.SmartNotes.Infrastructure.Extensions;
using System.Security.Cryptography;

namespace Xemio.SmartNotes.Infrastructure.Authentication
{
    /// <summary>
    /// Contains logic that authenticates a request.
    /// </summary>
    public class RequiresAuthenticationAttribute : AuthorizeAttribute
    {
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
            byte[] contentBytes = Encoding.UTF8.GetBytes(content);

            byte[] contentHash = new HMACSHA256(data.AuthenticationHash).ComputeHash(contentBytes);
            return Convert.ToBase64String(contentHash);
        }
        #endregion
    }
}
