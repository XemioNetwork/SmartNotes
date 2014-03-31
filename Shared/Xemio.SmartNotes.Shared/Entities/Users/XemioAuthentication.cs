using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xemio.SmartNotes.Shared.Entities.Users
{
    public class XemioAuthentication : AggregateRoot, IAuthentication
    {
        public XemioAuthentication()
        {
            this.Type = AuthenticationType.Xemio;
        }

        public AuthenticationType Type { get; set; }

        public string UserId { get; set; }

        public string Username { get; set; }

        public byte[] PasswordHash { get; set; }

        public byte[] Salt { get; set; }
    }
}
