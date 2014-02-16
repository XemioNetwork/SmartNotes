using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xemio.SmartNotes.Shared.Entities.Users;

namespace Xemio.SmartNotes.Server.Abstractions.Mailing
{
    public interface IEmailFactory
    {
        void SendEmailToUser(string mailTemplateName, User user, dynamic additionalData);
    }
}
