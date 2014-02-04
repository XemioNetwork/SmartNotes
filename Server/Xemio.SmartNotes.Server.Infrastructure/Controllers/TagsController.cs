using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Raven.Client;
using Xemio.SmartNotes.Server.Abstractions.Services;
using Xemio.SmartNotes.Server.Infrastructure.Exceptions;
using Xemio.SmartNotes.Server.Infrastructure.Extensions;
using Xemio.SmartNotes.Server.Infrastructure.Filters;
using Xemio.SmartNotes.Server.Infrastructure.RavenDB.Indexes;
using Xemio.SmartNotes.Shared.Entities.Users;
using Xemio.SmartNotes.Shared.Models;

namespace Xemio.SmartNotes.Server.Infrastructure.Controllers
{
    [RoutePrefix("Users/Authorized")]
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
        /// <param name="userService">The user service.</param>
        /// <param name="rightsService">The rights service.</param>
        public TagsController(IDocumentSession documentSession, IUserService userService, IRightsService rightsService)
            : base(documentSession, userService)
        {
            this._rightsService = rightsService;
        }
        #endregion

        #region Implementation of ITagsController
        /// <summary>
        /// Gets the tags from the <see cref="User"/>.
        /// </summary>
        [Route("Tags")]
        [RequiresAuthorization]
        public HttpResponseMessage GetTags(int count = 20)
        {
            var currentUser = this.UserService.GetCurrentUser();

            var tags = this.DocumentSession.Query<Tag, TagsByCount>()
                                           .Where(f => f.UserId == currentUser.Id)
                                           .OrderByDescending(f => f.Count)
                                           .Take(count)
                                           .ToList();
            
            return Request.CreateResponse(HttpStatusCode.Found, tags);
        }
        #endregion
    }
}
