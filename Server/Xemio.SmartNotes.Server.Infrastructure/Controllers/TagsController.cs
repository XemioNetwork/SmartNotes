using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Raven.Client;
using Xemio.SmartNotes.Models.Entities.Users;
using Xemio.SmartNotes.Models.Models;
using Xemio.SmartNotes.Server.Abstractions.Services;
using Xemio.SmartNotes.Server.Infrastructure.Exceptions;
using Xemio.SmartNotes.Server.Infrastructure.Extensions;
using Xemio.SmartNotes.Server.Infrastructure.Filters;
using Xemio.SmartNotes.Server.Infrastructure.Raven.Indexes;

namespace Xemio.SmartNotes.Server.Infrastructure.Controllers
{
    [RoutePrefix("Users/{userId:int}")]
    public class TagsController : BaseController
    {
        #region Fields
        private readonly IRightsService _rightsService;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="TagsController"/> class.
        /// </summary>
        /// <param name="documentSession">The document session.</param>
        /// <param name="rightsService">The rights service.</param>
        public TagsController(IDocumentSession documentSession, IRightsService rightsService)
            : base(documentSession)
        {
            this._rightsService = rightsService;
        }
        #endregion

        #region Implementation of ITagsController
        /// <summary>
        /// Gets the tags from the <see cref="User"/>.
        /// </summary>
        /// <param name="userId">The user id.</param>
        [Route("Tags")]
        [RequiresAuthorization]
        public HttpResponseMessage GetTags(int userId)
        {
            if (this._rightsService.HasCurrentUserTheUserId(userId) == false)
                throw new UnauthorizedException();

            string stringUserId = this.DocumentSession.Advanced.GetStringIdFor<User>(userId);

            var query = this.DocumentSession.Query<Tag, TagsByCount>().Where(f => f.UserId == stringUserId);

            var result = new List<Tag>();
            using (var enumerator = this.DocumentSession.Advanced.Stream(query))
            {
                while (enumerator.MoveNext())
                {
                    result.Add(enumerator.Current.Document);
                }
            }

            return Request.CreateResponse(HttpStatusCode.Found, result);
        }
        #endregion
    }
}
