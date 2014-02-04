using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Raven.Abstractions.Data;
using Raven.Client;
using Xemio.SmartNotes.Server.Abstractions.Services;
using Xemio.SmartNotes.Server.Infrastructure.Exceptions;
using Xemio.SmartNotes.Server.Infrastructure.Extensions;
using Xemio.SmartNotes.Server.Infrastructure.Filters;
using Xemio.SmartNotes.Shared.Entities.Users;
using Xemio.SmartNotes.Shared.Models;
using AssemblyResources = Xemio.SmartNotes.Server.Infrastructure.Properties.Resources;

namespace Xemio.SmartNotes.Server.Infrastructure.Controllers
{
    [RoutePrefix("Users/Authorized")]
    public class AvatarsController : BaseController
    {
        #region Constants
        private const string AvatarSuffix = "/Avatar";
        #endregion
        
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="AvatarsController"/> class.
        /// </summary>
        /// <param name="documentSession">The document session.</param>
        /// <param name="userService">The user service.</param>
        public AvatarsController(IDocumentSession documentSession, IUserService userService)
            : base(documentSession, userService)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Gets the current avatar.
        /// </summary>
        [Route("Avatar")]
        [RequiresAuthorization]
        public HttpResponseMessage GetAvatar(int width = 0, int height = 0)
        {
            string avatarId = this.GetAvatarAttachmentId();
            Attachment avatarData = this.DocumentStore.DatabaseCommands.GetAttachment(avatarId);

            Stream avatarStream = avatarData != null ? avatarData.Data() : AssemblyResources.DefaultAvatar.ToPngStream();

            using (var avatar = new Bitmap(avatarStream))
            using (var resizedAvatar = avatar.ResizeImage(width, height))
            {
                return Request.CreateImageStreamResponse(resizedAvatar.ToPngStream());
            }
        }
        /// <summary>
        /// Updates the avatar.
        /// </summary>
        /// <param name="avatar">The avatar.</param>
        [Route("Avatar")]
        [RequiresAuthorization]
        public HttpResponseMessage PutAvatar(CreateAvatar avatar)
        {
            if (avatar == null)
                throw new InvalidRequestException();
            if (avatar.AvatarBytes == null || avatar.AvatarBytes.Length == 0)
                throw new InvalidRequestException();
            
            string avatarId = this.GetAvatarAttachmentId();

            this.DocumentStore.DatabaseCommands.PutAttachment(avatarId, null, new MemoryStream(avatar.AvatarBytes), null);

            this.Logger.DebugFormat("Updated avatar '{0}'.", avatarId);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Returns the avatar attachment identifier.
        /// </summary>
        private string GetAvatarAttachmentId()
        {
            User currentUser = this.UserService.GetCurrentUser();
            return currentUser.Id + AvatarSuffix;
        }
        #endregion
    }
}
