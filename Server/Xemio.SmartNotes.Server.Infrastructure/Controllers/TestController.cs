using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Xemio.SmartNotes.Server.Infrastructure.Controllers
{
    public class TestController : ApiController
    {
        [Route("Test")]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
    }
}