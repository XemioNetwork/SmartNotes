using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xemio.SmartNotes.Client.Windows.Data.Events
{
    public class FolderSelectedEvent
    {
        public FolderSelectedEvent(string folderId)
        {
            this.FolderId = folderId;
        }

        public string FolderId { get; private set; }
    }
}
