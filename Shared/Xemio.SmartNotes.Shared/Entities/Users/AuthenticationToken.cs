using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xemio.SmartNotes.Shared.Entities.Users
{
    public class AuthenticationToken : AggregateRoot
    {
        public string Token { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public DateTimeOffset ValidUntil { get; set; }

        public string UserId { get; set; }

        public bool IsValid()
        {
            return this.ValidUntil >= DateTimeOffset.UtcNow;
        }
    }
}
