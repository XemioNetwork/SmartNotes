using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using Microsoft.Owin.Hosting;
using Xemio.SmartNotes.Server.Infrastructure;
using Xemio.SmartNotes.Shared.Extensions;

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
        static void Main(string[] args)
        {
            var options = new Options();

            if (Parser.Default.ParseArguments(args, options))
            {
                var startOptions = new StartOptions();
                startOptions.Urls.AddRange(options.Addresses);

                using (WebApp.Start<Startup>(startOptions))
                {
                    Console.WriteLine("Xemio SmartNotes Server started at {0}...", string.Join(", ", options.Addresses));
                    Console.WriteLine("Press <Enter> to close...");

                    Console.ReadLine();
                }
            }
            else
            {
                Console.ReadLine();
            }
        }
    }
}
