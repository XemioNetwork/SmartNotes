using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xemio.SmartNotes.Models.Entities.Users;

namespace Xemio.SmartNotes.Abstractions.Controllers
{
    public interface ITagsController : IController
    {
        /// <summary>
        /// Gets the tags from the <see cref="User"/>.
        /// </summary>
        /// <param name="userId">The user id.</param>
        Task<HttpResponseMessage> GetTags(int userId);
    }
}
