﻿using System;
using System.Threading;
using System.Threading.Tasks;
using CuttingEdge.Conditions;
using NLog.Conditions;
using Raven.Client;
using Xemio.SmartNotes.Server.Abstractions.Services;
using Xemio.SmartNotes.Shared.Entities.Users;

namespace Xemio.SmartNotes.Server.Infrastructure.Implementations.Services
{
    public class UserService : IUserService
    {
        #region Fields
        private readonly IDocumentSession _documentSession;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="documentSession">The document session.</param>
        public UserService(IDocumentSession documentSession)
        {
            Condition.Requires(documentSession, "documentSession")
                .IsNotNull();

            this._documentSession = documentSession;
        }
        #endregion

        #region Implementation of IUserService
        /// <summary>
        /// Returns the current user.
        /// </summary>
        public User GetCurrentUser(bool throwIfNoUser = true)
        {
            string userId = Thread.CurrentPrincipal.Identity.Name;

            if (string.IsNullOrWhiteSpace(userId) == false) 
                return this._documentSession.Load<User>(userId);

            if (throwIfNoUser)
                throw new InvalidOperationException("You can only access the current user when the controller-method has the 'RequiresAuthentication' attribute.");

            return null;
        }
        #endregion
    }
}
