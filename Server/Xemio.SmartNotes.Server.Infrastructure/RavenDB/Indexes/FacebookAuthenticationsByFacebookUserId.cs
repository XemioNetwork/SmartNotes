using System.Linq;
using Raven.Client.Indexes;
using Xemio.SmartNotes.Shared.Entities.Users;

namespace Xemio.SmartNotes.Server.Infrastructure.RavenDB.Indexes
{
    public class FacebookAuthenticationsByFacebookUserId : AbstractIndexCreationTask<FacebookAuthentication>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookAuthenticationsByFacebookUserId"/> class.
        /// </summary>
        public FacebookAuthenticationsByFacebookUserId()
        {
            this.Map = authentications => from auth in authentications
                                          select new
                                          {
                                              auth.FacebookUserId
                                          };
        }
        /// <summary>
        /// Gets the name of the index.
        /// </summary>
        public override string IndexName
        {
            get { return "FacebookAuthentications/ByFacebookUserId"; }
        }
    }
}