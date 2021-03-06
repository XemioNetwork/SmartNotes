﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CuttingEdge.Conditions;
using Raven.Client;
using Raven.Json.Linq;
using Xemio.SmartNotes.Server.Abstractions.Services;
using Xemio.SmartNotes.Server.Infrastructure.Extensions;
using Xemio.SmartNotes.Shared.Entities.Notes;
using Xemio.SmartNotes.Shared.Entities.Users;

namespace Xemio.SmartNotes.Server.Infrastructure.Implementations.Services
{
    public class ExampleDataService : IExampleDataService
    {
        #region Constants
        private const string ExampleDataCreated = "Example-Data-Created";
        #endregion

        #region Fields
        private readonly IDocumentSession _session;
        private readonly IUserService _userService;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ExampleDataService"/> class.
        /// </summary>
        /// <param name="session">The document session.</param>
        /// <param name="userService">The user service.</param>
        public ExampleDataService(IDocumentSession session, IUserService userService)
        {
            Condition.Requires(session, "session")
                .IsNotNull();
            Condition.Requires(userService, "userService")
                .IsNotNull();

            this._session = session;
            this._userService = userService;
        }
        #endregion

        #region Implementation of IExampleDataService
        /// <summary>
        /// Creates the example data for the current user.
        /// </summary>
        public void CreateExampleDataForCurrentUser()
        {
            User currentUser = this._userService.GetCurrentUser();

            var metadata = this._session.Advanced.GetMetadataFor(currentUser);

            RavenJToken token;
            if (metadata.TryGetValue(ExampleDataCreated, out token) && token.Value<bool>())
                return;
            
            this.CreateInternetFolder(currentUser);
            this.CreateLifeFolder(currentUser);

            metadata.Add(ExampleDataCreated, true);

            this._session.SaveChanges();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Creates the folder containing "Internet" things.
        /// </summary>
        /// <param name="currentUser">The current user.</param>
        private void CreateInternetFolder(User currentUser)
        {
            Condition.Requires(currentUser, "currentUser")
                .IsNotNull();

            var folder = new Folder
            {
                Name = "Internet",
                Tags = new Collection<string>
                {
                    "www",
                    "internet",
                    "google",
                    "facebook",
                    "microsoft"
                },
                UserId = currentUser.Id
            };
            this._session.Store(folder);

            this._session.SaveChanges();

            this.CreateGoogleNote(currentUser, folder);
        }
        /// <summary>
        /// Creates the note about Google.
        /// </summary>
        /// <param name="currentUser">The current user.</param>
        /// <param name="parentFolder">The parent folder.</param>
        private void CreateGoogleNote(User currentUser, Folder parentFolder)
        {
            Condition.Requires(currentUser, "currentUser")
                .IsNotNull();
            Condition.Requires(parentFolder, "parentFolder")
                .IsNotNull();

            var note = new Note
            {
                Name = "Google",
                Tags = new Collection<string>
                {
                    "search",
                    "engine"
                },
                FolderId = parentFolder.Id,
                UserId = currentUser.Id,
                Content = @"Google is an American multinational corporation specializing in Internet-related services and products.
These include search, cloud computing, software, and online advertising technologies.
Most of its profits are derived from AdWords.
Google was founded by Larry Page and Sergey Brin while they were Ph.D. students at Stanford University. 
Together they own about 16 percent of its shares. 
They incorporated Google as a privately held company on September 4, 1998. 
An initial public offering followed on August 19, 2004. 
Its mission statement from the outset was ""to organize the world's information and make it universally accessible and useful"", and its unofficial slogan was ""Don't be evil"".
In 2006 Google moved to headquarters in Mountain View, California, nicknamed the Googleplex."
            };

            this._session.Store(note);

            this._session.SaveChanges();
        }
        /// <summary>
        /// Creates a folder containing "Life" things.
        /// </summary>
        /// <param name="currentUser">The current user.</param>
        private void CreateLifeFolder(User currentUser)
        {
            Condition.Requires(currentUser, "currentUser")
                .IsNotNull();

            var folder = new Folder
            {
                Name = "Life",
                Tags = new Collection<string>
                {
                    "Life",
                    "Money",
                    "Work",
                },
                UserId = currentUser.Id
            };
            this._session.Store(folder);

            this._session.SaveChanges();
            
            this.CreateMoneySubFolder(currentUser, folder);
            this.CreateLifeHacksSubFolder(currentUser, folder);
        }
        /// <summary>
        /// Creates a folder containing "Money" things.
        /// </summary>
        /// <param name="currentUser">The current user.</param>
        /// <param name="parentFolder">The parent folder.</param>
        private void CreateMoneySubFolder(User currentUser, Folder parentFolder)
        {
            Condition.Requires(currentUser, "currentUser")
                .IsNotNull();
            Condition.Requires(parentFolder, "parentFolder")
                .IsNotNull();

            var folder = new Folder
            {
                Name = "Money",
                Tags = new Collection<string>
                {
                    "Earnings",
                    "Dollar",
                    "Euro"
                },
                ParentFolderId = parentFolder.Id,
                UserId = currentUser.Id
            };

            this._session.Store(folder);

            this._session.SaveChanges();
        }
        /// <summary>
        /// Creates a folder containing "Life-Hacks".
        /// </summary>
        /// <param name="currentUser">The current user.</param>
        /// <param name="parentFolder">The parent folder.</param>
        private void CreateLifeHacksSubFolder(User currentUser, Folder parentFolder)
        {
            Condition.Requires(currentUser, "currentUser")
                .IsNotNull();
            Condition.Requires(parentFolder, "parentFolder")
                .IsNotNull();

            var folder = new Folder
            {
                Name = "Life-Hacks",
                Tags = new Collection<string>
                {
                    "Lifehacks",
                    "Usefull",
                    "Tricks"
                },
                ParentFolderId = parentFolder.Id,
                UserId = currentUser.Id
            };

            this._session.Store(folder);

            this._session.SaveChanges();
        }
        #endregion
    }
}
