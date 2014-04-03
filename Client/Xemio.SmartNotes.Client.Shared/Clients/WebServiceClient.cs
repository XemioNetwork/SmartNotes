namespace Xemio.SmartNotes.Client.Shared.Clients
{
    public class WebServiceClient
    {
        #region Properties
        /// <summary>
        /// Gets the <see cref="IFoldersClient"/>.
        /// </summary>
        public IFoldersClient Folders { get; private set; }
        /// <summary>
        /// Gets the <see cref="INotesClient"/>.
        /// </summary>
        public INotesClient Notes { get; private set; }
        /// <summary>
        /// Gets the <see cref="IUsersClient"/>.
        /// </summary>
        public IUsersClient Users { get; private set; }
        /// <summary>
        /// Gets the <see cref="IPasswordResetClient"/>.
        /// </summary>
        public IPasswordResetClient PasswordResets { get; private set; }
        /// <summary>
        /// Gets the <see cref="IAvatarsClient"/>.
        /// </summary>
        public IAvatarsClient Avatars { get; private set; }
        /// <summary>
        /// Gets the <see cref="ITagsClient"/>.
        /// </summary>
        public ITagsClient Tags { get; private set; }
        /// <summary>
        /// Gets the <see cref="ITokenClient"/>.
        /// </summary>
        public ITokenClient Tokens { get; private set; }
        /// <summary>
        /// Gets the <see cref="Session"/>.
        /// </summary>
        public Session Session { get; private set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="WebServiceClient"/> class.
        /// </summary>
        /// <param name="folders">The folders Client.</param>
        /// <param name="notes">The notes Client.</param>
        /// <param name="users">The users Client.</param>
        /// <param name="passwordResets">The password resets Client.</param>
        /// <param name="tagsClient">The tags Client.</param>
        /// <param name="avatarsClient">The avatars Client.</param>
        /// <param name="tokenClient">The token Client.</param>
        /// <param name="session">The session.</param>
        public WebServiceClient(IFoldersClient folders, INotesClient notes, IUsersClient users, IPasswordResetClient passwordResets, ITagsClient tagsClient, IAvatarsClient avatarsClient, ITokenClient tokenClient, Session session)
        {
            this.Folders = folders;
            this.Notes = notes;
            this.Users = users;
            this.PasswordResets = passwordResets;
            this.Avatars = avatarsClient;
            this.Tags = tagsClient;
            this.Tokens = tokenClient;
            this.Session = session;
        }
        #endregion
    }
}
