using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Raven.Client.Indexes;
using Xemio.SmartNotes.Shared.Entities.Users;

namespace Xemio.SmartNotes.Server.Infrastructure.RavenDB.Indexes
{
    public class XemioAuthenticationsByUserId : AbstractIndexCreationTask<XemioAuthentication>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XemioAuthenticationsByUserId"/> class.
        /// </summary>
        public XemioAuthenticationsByUserId()
        {
            this.Map = authentications => from auth in authentications
                                          select new
                                          {
                                              auth.UserId
                                          };
        }
        /// <summary>
        /// Gets the name of the index.
        /// </summary>
        public override string IndexName
        {
            get { return "XemioAuthentications/ByUserId"; }
        }
    }
}