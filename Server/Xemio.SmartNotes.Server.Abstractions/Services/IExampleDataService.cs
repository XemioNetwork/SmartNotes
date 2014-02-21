using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xemio.SmartNotes.Shared.Entities.Users;

namespace Xemio.SmartNotes.Server.Abstractions.Services
{
    public interface IExampleDataService : IService
    {
        /// <summary>
        /// Creates the example data for the current user.
        /// </summary>
        void CreateExampleDataForCurrentUser();
    }
}
