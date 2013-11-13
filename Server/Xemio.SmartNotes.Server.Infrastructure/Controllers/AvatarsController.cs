using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Raven.Abstractions.Data;
using Raven.Client;
using Xemio.SmartNotes.Abstractions.Controllers;
using Xemio.SmartNotes.Models.Entities.Users;
using Xemio.SmartNotes.Server.Abstractions.Services;
using Xemio.SmartNotes.Server.Infrastructure.Extensions;
using Xemio.SmartNotes.Server.Infrastructure.Filters;
using Xemio.SmartNotes.Server.Infrastructure.Properties;

namespace Xemio.SmartNotes.Server.Infrastructure.Controllers
{
    [RoutePrefix("Users/Authorized")]
    public class AvatarsController : BaseController, IAvatarsController
    {
        #region Constants
        private const string AvatarSuffix = "/Avatar";
        #endregion

        #region Fields
        private readonly IUserService _userService;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="AvatarsController"/> class.
        /// </summary>
        /// <param name="documentSession">The document session.</param>
        /// <param name="userService">The user service.</param>
        public AvatarsController(IAsyncDocumentSession documentSession, IUserService userService)
            : base(documentSession)
        {
            this._userService = userService;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Gets the current avatar.
        /// </summary>
        [Route("Avatar")]
        [RequiresAuthorization]
        public async Task<HttpResponseMessage> GetAvatar()
        {
            User currentUser = await this._userService.GetCurrentUser();
            Attachment avatar = this.DocumentStore.DatabaseCommands.GetAttachment(currentUser.Id += AvatarSuffix);

            if (avatar == null)
            {
                MemoryStream stream = new MemoryStream();
                Resources.DefaultAvatar.Save(stream, ImageFormat.Png);

                return Request.CreateImageStreamResponse(stream);
            }

            return Request.CreateImageStreamResponse(avatar.Data());
        }
        /// <summary>
        /// Updates the avatar.
        /// </summary>
        /// <param name="avatar">The avatar.</param>
        [Route("Avatar")]
        [RequiresAuthorization]
        public async Task<HttpResponseMessage> PutAvatar([FromBody]Stream avatar)
        {
            User currentUser = await this._userService.GetCurrentUser();

            this.DocumentStore.DatabaseCommands.PutAttachment(currentUser.Id += AvatarSuffix, null, avatar, null);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
        #endregion
    }
}
