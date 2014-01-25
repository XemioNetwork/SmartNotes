using System;
using System.Net;
using System.Net.Cache;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Castle.Core.Logging;
using Newtonsoft.Json;
using Xemio.SmartNotes.Abstractions.Authorization;

namespace Xemio.SmartNotes.Client.Shared.WebService
{
    /// <summary>
    /// A base class for all webservice clients.
    /// </summary>
    public abstract class BaseClient : IDisposable
    {
        #region Properties
        /// <summary>
        /// Gets the client.
        /// </summary>
        protected HttpClient Client { get; private set; }
        /// <summary>
        /// Gets the session.
        /// </summary>
        protected Session Session { get; private set; }
        /// <summary>
        /// Gets the logger.
        /// </summary>
        protected ILogger Logger { get; private set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseClient"/> class.
        /// </summary>
        /// <param name="baseAddress">The base address.</param>
        /// <param name="session">The application session.</param>
        protected BaseClient(string baseAddress, Session session)
        {
            this.Logger = NullLogger.Instance;

            this.Session = session;
            this.Client = new HttpClient
                              {
                                  BaseAddress = new Uri(baseAddress)
                              };
        }
        #endregion

        #region Methods
        /// <summary>
        /// Creates a new request.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="relativeUri">The relative URI.</param>
        /// <param name="content">The content.</param>
        protected HttpRequestMessage CreateRequest(HttpMethod method, string relativeUri, object content = null)
        {
            DateTimeOffset requestDate = this.GetCurrentDate();
            string contentString = content != null ? JsonConvert.SerializeObject(content) : null;
            string authorizationHash = AuthorizationHash.Create(this.Session.Username, this.Session.Password, requestDate, contentString);

            var request = new HttpRequestMessage(method, relativeUri)
                              {
                                  Headers =
                                  {
                                      Authorization = new AuthenticationHeaderValue("Xemio", string.Format("{0}:{1}", this.Session.Username, authorizationHash)),
                                      Date = requestDate
                                  }
                              };

            if (string.IsNullOrWhiteSpace(contentString) == false)
                request.Content = new StringContent(contentString, Encoding.UTF8, "application/json");

            request.Headers.AcceptLanguage.Add(new StringWithQualityHeaderValue(Thread.CurrentThread.CurrentUICulture.Name));

            return request;
        }
        /// <summary>
        /// Sends specified <paramref name="request"/> asynchronously.
        /// </summary>
        /// <param name="request">The request.</param>
        protected async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            try
            {
                return await this.Client.SendAsync(request);
            }
            catch (Exception exception)
            {
                this.Logger.Error("Error while executing request.", exception);
                return this.CreateNotReachableResponse();
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Returns the current date without milliseconds.
        /// We need to remove the milliseconds because the HTTP Header does not transport them, so we lose them on the server and the authentication fails.
        /// </summary>
        private DateTimeOffset GetCurrentDate()
        {
            var current = DateTimeOffset.UtcNow;
            return new DateTimeOffset(current.Year, current.Month, current.Day, current.Hour, current.Minute, current.Second, current.Offset);
        }
        /// <summary>
        /// Creates the response when you can't reach the web-service.
        /// </summary>
        private HttpResponseMessage CreateNotReachableResponse()
        {
            return new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent(ClientMessages.NotReachable)
            };
        }
        #endregion

        #region Implementation of IDisposable
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Client.Dispose();
        }
        #endregion
    }
}