using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xemio.SmartNotes.Shared.Entities.Users
{
    public interface IAuthentication
    {
        AuthenticationType Type { get; set; }

        string UserId { get; set; }
    }

    public enum AuthenticationType
    {
        Xemio,
        Facebook
    }
}
