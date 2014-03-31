using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Xemio.SmartNotes.Shared.Entities.Users;

namespace Xemio.SmartNotes.Shared.Models
{
    public class CreateUser
    {
        public string EmailAddress { get; set; }

        public string PreferredLanguage { get; set; }

        public string TimeZoneId { get; set; }

        public AuthenticationType AuthenticationType { get; set; }

        public JObject AuthenticationData { get; set; }
    }
}
