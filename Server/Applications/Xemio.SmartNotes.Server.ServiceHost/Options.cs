using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text;

namespace Xemio.SmartNotes.Server.ServiceHost
{
    public class Options
    {
        [OptionList('a', "addresses", '|', HelpText = "The addresses where the webservice will be accessible.", Required = true)]
        public IList<string> Addresses { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this, f => HelpText.DefaultParsingErrorsHandler(this, f));
        }
    }
}
