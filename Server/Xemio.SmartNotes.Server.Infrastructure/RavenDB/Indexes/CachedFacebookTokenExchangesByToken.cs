using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Raven.Client.Indexes;
using Xemio.SmartNotes.Shared.Entities.Users;

namespace Xemio.SmartNotes.Server.Infrastructure.RavenDB.Indexes
{
    public class CachedFacebookTokenExchangesByToken : AbstractIndexCreationTask<CachedFacebookTokenExchange>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CachedFacebookTokenExchangesByToken"/> class.
        /// </summary>
        public CachedFacebookTokenExchangesByToken()
        {
            this.Map = tokenExchanges => from exchange in tokenExchanges
                                         select new
                                         {
                                             exchange.Token
                                         };
        }
        /// <summary>
        /// Gets the name of the index.
        /// </summary>
        public override string IndexName
        {
            get { return "CachedFacebookTokenExchanges/ByToken"; }
        }
    }
}