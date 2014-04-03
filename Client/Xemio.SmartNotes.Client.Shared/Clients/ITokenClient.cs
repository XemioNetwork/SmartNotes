using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xemio.SmartNotes.Shared.Models;

namespace Xemio.SmartNotes.Client.Shared.Clients
{
    public interface ITokenClient : IClient
    {
        Task<HttpResponseMessage> PostXemio(string username, string password);

        Task<HttpResponseMessage> PostFacebook(string token, string redirectUrl);
    }
}
