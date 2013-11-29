using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Xemio.SmartNotes.Client.Abstractions.Tasks
{
    public interface ITask
    {
        string DisplayName { get; }

        Task Execute();
    }
}
