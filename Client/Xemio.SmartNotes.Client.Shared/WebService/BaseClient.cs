using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
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

            this.SetLanguageHeader();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Sets the authentication header for the given content.
        /// </summary>
        /// <param name="content">The content.</param>
        protected void SetAuthenticationHeader(string content = "")
        {
            string contentHashString = AuthorizationHash.Create(this.Session.Username, this.Session.Password, content);

            this.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Xemio", string.Format("{0}:{1}", this.Session.Username, contentHashString));
        }
        /// <summary>
        /// Sets the language header.
        /// </summary>
        protected void SetLanguageHeader()
        {
            this.Client.DefaultRequestHeaders.AcceptLanguage.Clear();

            this.Client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue(Thread.CurrentThread.CurrentUICulture.Name));
        }
        #endregion
    }
}