using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xemio.SmartNotes.Shared.Entities.Mailing
{
    public class EmailSendType
    {
        public bool Immediate { get; set; }

        public DateTimeOffset? AtTime { get; set; }
    }
}
