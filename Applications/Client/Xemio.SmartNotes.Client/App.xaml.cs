using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Xemio.SmartNotes.Abstractions.Controllers;
using Xemio.SmartNotes.Client.WebServices;
using Xemio.SmartNotes.Entities.Users;
using Xemio.SmartNotes.Models.Users;

namespace Xemio.SmartNotes.Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
        }
    }
}
