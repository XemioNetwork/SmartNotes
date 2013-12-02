using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using Xemio.SmartNotes.Abstractions.Authorization;

namespace Xemio.SmartNotes.Client.Shared.WebService
{
    /// <summary>
    /// A base class for all webservice clients.
    /// </summary>
    public abstract class BaseClient
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
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseClient"/> class.
        /// </summary>
        /// <param name="baseAddress">The base address.</param>
        /// <param name="session">The application session.</param>
        protected BaseClient(string baseAddress, Session session)
        {
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
            string contentString = content != null ? JsonConvert.SerializeObject(content) : string.Empty;
            string authorizationHash = AuthorizationHash.Create(this.Session.Username, this.Session.Password, contentString);

            var request = new HttpRequestMessage(method, relativeUri)
                              {
                                  Headers =
                                  {
                                      Authorization = new AuthenticationHeaderValue("Xemio", string.Format("{0}:{1}", this.Session.Username, authorizationHash)),
                                  }
                              };

            if (string.IsNullOrWhiteSpace(contentString) == false)
                request.Content = new StringContent(contentString, Encoding.UTF8, "application/json");

            request.Headers.AcceptLanguage.Add(new StringWithQualityHeaderValue(Thread.CurrentThread.CurrentUICulture.Name));

            return request;
        }
        #endregion
    }
}