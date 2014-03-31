using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Xemio.SmartNotes.Shared.Entities.Users;

namespace Xemio.SmartNotes.Shared.Models
{
    public class CreateToken
    {
        public AuthenticationType Type { get; set; }

        public JObject AuthenticationData { get; set; }
    }
}
