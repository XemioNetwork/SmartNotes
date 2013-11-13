using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;
using Xemio.SmartNotes.Server.Infrastructure;

namespace Xemio.SmartNotes.Server.ConsoleHost
{
    /// <summary>
    /// Contains the main entry point for the application.
    /// </summary>
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            const string baseAddress = "http://localhost/";

            using (WebApp.Start<Startup>(baseAddress))
            {
                Console.WriteLine("Xemio SmartNotes Server started at {0}...", baseAddress);
                Console.WriteLine("Press <Enter> to close...");

                Console.ReadLine();
            }
        }
    }
}
