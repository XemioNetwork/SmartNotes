using System.Linq;
using Raven.Client.Indexes;
using Xemio.SmartNotes.Shared.Entities.Users;

namespace Xemio.SmartNotes.Server.Infrastructure.RavenDB.Indexes
{
    public class UsersByEmailAddress : AbstractIndexCreationTask<User>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UsersByEmailAddress"/> class.
        /// </summary>
        public UsersByEmailAddress()
        {
            this.Map = users => from user in users
                                select new
                                {
                                    user.EmailAddress
                                };
        }
        /// <summary>
        /// Gets the name of the index.
        /// </summary>
        public override string IndexName
        {
            get { return "Users/ByEmailAddress"; }
        }
    }
}