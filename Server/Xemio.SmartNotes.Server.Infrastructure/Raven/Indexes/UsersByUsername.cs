using System.Linq;
using Raven.Client.Indexes;
using Xemio.SmartNotes.Models.Entities.Users;

namespace Xemio.SmartNotes.Server.Infrastructure.Raven.Indexes
{
    /// <summary>
    /// Enables seaching for a <see cref="User"/> by it's username.
    /// </summary>
    public class UsersByUsername : AbstractIndexCreationTask<User>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UsersByUsername"/> class.
        /// </summary>
        public UsersByUsername()
        {
            this.Map = users => from user in users
                                select new
                                           {
                                               user.Username
                                           };
        }

        /// <summary>
        /// Gets the name of the index.
        /// </summary>
        public override string IndexName
        {
            get { return "Users/ByUsername"; }
        }
    }
}
