using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;
using Xemio.SmartNotes.Server.Infrastructure;
using Xemio.SmartNotes.Shared.Extensions;

namespace Xemio.SmartNotes.Server.Host
{
    class Program
    {
        static void Main(string[] args)
        {
            var startOptions = new StartOptions();
            startOptions.Urls.AddRange(ConfigurationManager.AppSettings["XemioNotes/Addresses"].Split('|'));

            using (WebApp.Start<Startup>(startOptions))
            {
                Console.WriteLine("Xemio Notes Web-API started.");
                Console.WriteLine("Press any key to close.");

                Console.ReadLine();
            }
        }
    }
}
