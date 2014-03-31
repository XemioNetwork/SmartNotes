using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xemio.SmartNotes.Shared.Entities.Users
{
    public class FacebookAuthentication : AggregateRoot, IAuthentication
    {
        public FacebookAuthentication()
        {
            this.Type = AuthenticationType.Facebook;
        }

        public AuthenticationType Type { get; set; }

        public string UserId { get; set; }

        public string FacebookUserId { get; set; }
    }
}
