using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using Castle.Core.Logging;
using Raven.Client;
using Xemio.SmartNotes.Entities.Users;

namespace Xemio.SmartNotes.Infrastructure.Controllers
{
    /// <summary>
    /// A basic <see cref="ApiController"/> providing access to an <see cref="IAsyncDocumentSession"/> and and <see cref="ILogger"/>.
    /// </summary>
    public abstract class BaseController : ApiController
    {
        private ILogger _logger = NullLogger.Instance;

        #region Properties
        /// <summary>
        /// Gets or sets the document session.
        /// </summary>
        public IAsyncDocumentSession DocumentSession { get; private set; }
        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        public ILogger Logger
        {
            get { return _logger; }
            set { _logger = value; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseController"/> class.
        /// </summary>
        /// <param name="documentSession">The document session.</param>
        protected BaseController(IAsyncDocumentSession documentSession)
        {
            this.DocumentSession = documentSession;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Executes asynchronously a single HTTP operation.
        /// </summary>
        /// <param name="controllerContext">The controller context for a single HTTP operation.</param>
        /// <param name="cancellationToken">The cancellation token assigned for the HTTP operation.</param>
        public override async Task<HttpResponseMessage> ExecuteAsync(HttpControllerContext controllerContext, CancellationToken cancellationToken)
        {
            this.HandleAcceptLanguageHeader(controllerContext);

            var response = await base.ExecuteAsync(controllerContext, cancellationToken);

            using (this.DocumentSession)
            {
                await this.DocumentSession.SaveChangesAsync();
            }

            return response;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Handles the accept language header and sets the culture of the current <see cref="Thread"/>.
        /// </summary>
        /// <param name="context">The context.</param>
        private void HandleAcceptLanguageHeader(HttpControllerContext context)
        {
            if (context.Request.Headers.AcceptLanguage != null && context.Request.Headers.AcceptLanguage.Count > 0)
            {
                string language = context.Request.Headers.AcceptLanguage.First().Value;
                CultureInfo culture = CultureInfo.CreateSpecificCulture(language);

                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentUICulture = culture;
            }
        }
        #endregion
    }
}
