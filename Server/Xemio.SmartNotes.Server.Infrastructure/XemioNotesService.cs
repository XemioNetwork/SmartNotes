using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;
using Xemio.SmartNotes.Shared.Extensions;

namespace Xemio.SmartNotes.Server.Infrastructure
{
    public class XemioNotesService
    {
        #region Fields
        private readonly string[] _addresses;
        private IDisposable _service;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="XemioNotesService"/> class.
        /// </summary>
        /// <param name="addresses">The addresses.</param>
        public XemioNotesService(params string[] addresses)
        {
            this._addresses = addresses;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Starts the <c>Xemio Notes</c> service.
        /// </summary>
        public void Start()
        {
            var startOptions = new StartOptions();
            startOptions.Urls.AddRange(this._addresses);

            this._service = WebApp.Start<Startup>(startOptions);
        }
        /// <summary>
        /// Stops the <c>Xemio Notes</c> service.
        /// </summary>
        public void Stop()
        {
            if (this._service != null)
                this._service.Dispose();
        }
        #endregion
    }
}
