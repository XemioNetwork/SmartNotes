using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;
using Xemio.SmartNotes.Server.Infrastructure;

namespace Xemio.SmartNotes.Server.ServiceHost
{
    /// <summary>
    /// Starts the SmartNotes webservice inside a windows service.
    /// </summary>
    partial class SmartNotesService : ServiceBase
    {
        #region Fields
        private IDisposable _webService;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="SmartNotesService"/> class.
        /// </summary>
        public SmartNotesService()
        {
            InitializeComponent();
        }
        #endregion

        #region Overrides of ServiceBase
        /// <summary>
        /// When implemented in a derived class, executes when a Start command is sent to the service by the Service Control Manager (SCM) or when the operating system starts (for a service that starts automatically). Specifies actions to take when the service starts.
        /// </summary>
        /// <param name="args">Data passed by the start command.</param>
        protected override void OnStart(string[] args)
        {
            const string baseAddress = "http://localhost/";

            this._webService = WebApp.Start<Startup>(baseAddress);
        }
        /// <summary>
        /// When implemented in a derived class, executes when a Stop command is sent to the service by the Service Control Manager (SCM). Specifies actions to take when a service stops running.
        /// </summary>
        protected override void OnStop()
        {
            this._webService.Dispose();
        }
        #endregion
    }
}
