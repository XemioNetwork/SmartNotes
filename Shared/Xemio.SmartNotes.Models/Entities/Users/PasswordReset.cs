using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xemio.SmartNotes.Models.Entities.Users
{
    /// <summary>
    /// Contains information about a requested password reset.
    /// </summary>
    public class PasswordReset
    {
        public string UserId { get; set; }
        public string Secret { get; set; }
        public DateTimeOffset RequestedAt { get; set; }
        public bool PasswordWasReset { get; set; }
    }
}
