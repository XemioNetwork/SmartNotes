using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace Xemio.SmartNotes.Client.Events
{
    public class ChangeCurrentScreenEvent
    {
        public ChangeCurrentScreenEvent(Screen nextScreen)
        {
            this.NextScreen = nextScreen;
        }

        public Screen NextScreen { get; private set; }
    }
}
