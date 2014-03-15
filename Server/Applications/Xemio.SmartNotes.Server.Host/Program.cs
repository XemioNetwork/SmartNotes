using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;
using Xemio.SmartNotes.Server.Infrastructure;

namespace Xemio.SmartNotes.Server.Host
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(x =>
            {
                x.Service<XemioNotesService>(s =>
                {
                    s.ConstructUsing(name => new XemioNotesService(ConfigurationManager.AppSettings["XemioNotes/Addresses"].Split('|')));
                    s.WhenStarted(f => f.Start());
                    s.WhenStopped(f => f.Stop());
                });

                x.RunAsLocalSystem();
                x.StartAutomatically();

                x.SetDescription("The HTTP API for Xemio Notes.");
                x.SetDisplayName("Xemio Notes HTTP API");
                x.SetServiceName("XemioNotesHTTPAPI");

                x.UseNLog();
            });
        }
    }
}
