using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raven.Client.Indexes;
using Xemio.SmartNotes.Entities.Users;

namespace Xemio.SmartNotes.Infrastructure.Raven.Indexes
{
    /// <summary>
    /// Enables querying for <see cref="UserAuthentication"/>s by their <see cref="UserAuthentication.UserId"/> property.
    /// </summary>
    public class UserAuthenticationsByUserId : AbstractIndexCreationTask<UserAuthentication>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserAuthenticationsByUserId"/> class.
        /// </summary>
        public UserAuthenticationsByUserId()
        {
            this.Map = authentications => from authentication in authentications
                                          select new {authentication.UserId};
        }

        /// <summary>
        /// Gets the name of the index.
        /// </summary>
        public override string IndexName
        {
            get { return "UserAuthentications/ByUserId"; }
        }
    }
}
