using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xemio.SmartNotes.Shared.Entities.Mailing
{
    public class EmailAttachment
    {
        public string Name { get; set; }

        public string FilePath { get; set; }

        public bool IsInline { get; set; }

        public string MediaType { get; set; }
    }
}
