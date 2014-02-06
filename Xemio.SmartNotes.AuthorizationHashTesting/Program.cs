using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xemio.SmartNotes.Shared.Authorization;

namespace Xemio.SmartNotes.AuthorizationHashTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            var hash = AuthorizationHash.Create("haefele", "123456", new DateTimeOffset(2014, 2, 6, 12, 0, 30, TimeSpan.Zero), "asdf").Result;
            Console.WriteLine(hash);

            Console.ReadLine();
        }
    }
}
