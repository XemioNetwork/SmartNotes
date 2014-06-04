using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Windows.Media.Imaging;
using Caliburn.Micro;
using Castle.Core.Logging;
using Xemio.SmartNotes.Client.Shared.Clients;
using Xemio.SmartNotes.Client.Windows.Data.Events;
using Xemio.SmartNotes.Client.Windows.Data.Exceptions;
using Xemio.SmartNotes.Client.Windows.Extensions;
using Xemio.SmartNotes.Shared.Models;

namespace Xemio.SmartNotes.Client.Windows.Implementations.Tasks
{
    public class ChangeAvatarTask : BaseTask
    {
        #region Fields
        private readonly IEventAggregator _eventAggregator;
        private readonly WebServiceClient _webServiceClient;

        private BitmapImage _newAvatar;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the new avatar.
        /// </summary>
        public BitmapImage NewAvatar
        {
            get { return this._newAvatar; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();

                this._newAvatar = value;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeAvatarTask"/> class.
        /// </summary>
        /// <param name="eventAggregator">The event aggregator.</param>
        /// <param name="webServiceClient">The web service client.</param>
        public ChangeAvatarTask(IEventAggregator eventAggregator, WebServiceClient webServiceClient)
        {
            this._eventAggregator = eventAggregator;
            this._webServiceClient = webServiceClient;
        }
        #endregion

        #region Overrides of BaseTask
        /// <summary>
        /// Gets the display name for this task.
        /// </summary>
        public override string DisplayName
        {
            get { return TaskMessages.ChangeAvatarTask; }
        }
        /// <summary>
        /// Executes this task.
        /// </summary>
        public override async Task Execute()
        {
            using (var stream = this.NewAvatar.ToPngMemoryStream())
            {
                var data = new CreateAvatar
                            {
                                AvatarBytes = stream.ToArray()
                            };

                HttpResponseMessage response = await this._webServiceClient.Avatars.PutAvatar(data);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    this._eventAggregator.PublishOnUIThread(new AvatarChangedEvent());
                }
                else
                {
                    var error = await response.Content.ReadAsAsync<HttpError>();
                    this.Logger.ErrorFormat("Error while changing avatar from user '{0}': {1}", this._webServiceClient.Session.User.Id, error.Message);

                    throw new GenericException(TaskMessages.ChangeAvatarTaskFailed);
                }
            }
        }
        #endregion
    }
}
