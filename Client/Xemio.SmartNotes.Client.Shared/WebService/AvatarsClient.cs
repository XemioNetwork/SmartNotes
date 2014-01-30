﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Xemio.SmartNotes.Client.Abstractions.Clients;
using Xemio.SmartNotes.Models.Models;

namespace Xemio.SmartNotes.Client.Shared.WebService
{
    public class AvatarsClient : BaseClient, IAvatarsClient
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="AvatarsClient"/> class.
        /// </summary>
        /// <param name="baseAddress">The base address.</param>
        /// <param name="session">The application session.</param>
        public AvatarsClient(string baseAddress, Session session)
            : base(baseAddress, session)
        {
        }
        #endregion

        #region Implementation of IAvatarsController
        /// <summary>
        /// Gets the current avatar.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public async Task<HttpResponseMessage> GetAvatar(int width = 0, int height = 0)
        {
            var query = new HttpQueryBuilder();
            query.AddParameter("width", width);
            query.AddParameter("height", height);

            var request = await this.CreateRequest(HttpMethod.Get, string.Format("Users/Authorized/Avatar{0}", query));
            return await this.SendAsync(request);
        }
        /// <summary>
        /// Updates the avatar.
        /// </summary>
        /// <param name="avatar">The avatar.</param>
        public async Task<HttpResponseMessage> PutAvatar(CreateAvatar avatar)
        {
            var request = await this.CreateRequest(HttpMethod.Get, "Users/Authorized/Avatar", avatar);
            return await this.SendAsync(request);
        }
        #endregion
    }
}
