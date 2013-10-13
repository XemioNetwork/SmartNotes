using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http.SelfHost;
using Xemio.SmartNotes.Infrastructure.Configs;

namespace Xemio.SmartNotes.ConsoleServer
{
    /// <summary>
    /// Contains startup logic for the console-server.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The startup method.
        /// </summary>
        public static void Main()
        {
            var config = new HttpSelfHostConfiguration("http://localhost");

            RoutingConfig.Configure(config);
            SimpleInjectorConfig.Configure(config);
            FilterConfig.Configure(config);

            using (var server = new HttpSelfHostServer(config))
            {
                server.OpenAsync().Wait();
                Console.WriteLine("Xemio SmartNotes Server started at {0}...", config.BaseAddress);
                Console.WriteLine("Press <Enter> to close...");

                Console.ReadLine();
            }
        }
    }
}
