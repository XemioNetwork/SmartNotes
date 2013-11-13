using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xemio.SmartNotes.Models.Models
{
    public class CreateUser
    {
        public string Username { get; set; }
        public string EMailAddress { get; set; }
        public byte[] AuthorizationHash { get; set; }
    }
}
