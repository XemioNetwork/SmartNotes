using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Xemio.SmartNotes.Shared.Entities.Users;
using Xemio.SmartNotes.Shared.Models;

namespace Xemio.SmartNotes.Client.Shared.Clients
{
    public class TokenClient : BaseClient, ITokenClient
    {
        public TokenClient(string baseAddress, Session session) : base(baseAddress, session)
        {
        }

        #region Implementation of ITokenClient

        public Task<HttpResponseMessage> PostXemio(string username, string password)
        {
            return this.Post(new CreateToken
            {
                Type = AuthenticationType.Xemio,
                AuthenticationData = new JObject
                {
                    { "Username", username },
                    { "Password", password }
                }
            });
        }

        public Task<HttpResponseMessage> PostFacebook(string token, string redirectUrl)
        {
            return this.Post(new CreateToken
            {
                Type = AuthenticationType.Facebook,
                AuthenticationData = new JObject
                {
                    {"Code", token},
                    {"RedirectUri", redirectUrl}
                }
            });
        }
        #endregion


        #region Private Methods
        public Task<HttpResponseMessage> Post(CreateToken createToken)
        {
            var request = this.CreateRequest(HttpMethod.Post, "Token", createToken);
            return this.SendAsync(request);
        }
        #endregion
    }
}
