using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xemio.SmartNotes.Shared.Entities.Users
{
    public class CachedFacebookTokenExchange : AggregateRoot
    {
        public string Token { get; set; }
        public string AccessToken { get; set; }
    }
}
