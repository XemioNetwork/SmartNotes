﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xemio.SmartNotes.Client.Abstractions.Clients;
using Xemio.SmartNotes.Models.Entities.Users;

namespace Xemio.SmartNotes.Client.Shared.WebService
{
    public class TagsClient : BaseClient, ITagsClient
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="TagsClient"/> class.
        /// </summary>
        /// <param name="baseAddress">The base address.</param>
        /// <param name="session">The application session.</param>
        public TagsClient(string baseAddress, Session session)
            : base(baseAddress, session)
        {
        }
        #endregion

        #region Implementation of ITagsController
        /// <summary>
        /// Gets the tags from the <see cref="User" />.
        /// </summary>
        public Task<HttpResponseMessage> GetTags()
        {
            var request = this.CreateRequest(HttpMethod.Get, "Users/Authorized/Tags");
            return this.Client.SendAsync(request);
        }
        #endregion
    }
}
