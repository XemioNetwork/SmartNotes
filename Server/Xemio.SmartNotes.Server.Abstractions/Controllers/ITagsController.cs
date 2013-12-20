using System.Net.Http;
using Xemio.SmartNotes.Models.Entities.Users;

namespace Xemio.SmartNotes.Server.Abstractions.Controllers
{
    public interface ITagsController : IController
    {
        /// <summary>
        /// Gets the tags from the <see cref="User"/>.
        /// </summary>
        /// <param name="userId">The user id.</param>
        HttpResponseMessage GetTags(int userId);
    }
}
