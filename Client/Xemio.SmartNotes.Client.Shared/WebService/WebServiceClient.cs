using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xemio.SmartNotes.Abstractions.Controllers;

namespace Xemio.SmartNotes.Client.Shared.WebService
{
    public class WebServiceClient
    {
        #region Properties
        /// <summary>
        /// Gets the <see cref="IFoldersController"/>.
        /// </summary>
        public IFoldersController Folders { get; private set; }
        /// <summary>
        /// Gets the <see cref="INotesController"/>.
        /// </summary>
        public INotesController Notes { get; private set; }
        /// <summary>
        /// Gets the <see cref="IUsersController"/>.
        /// </summary>
        public IUsersController Users { get; private set; }
        /// <summary>
        /// Gets the <see cref="IPasswordResetController"/>.
        /// </summary>
        public IPasswordResetController PasswordResets { get; private set; }
        /// <summary>
        /// Gets the <see cref="Session"/>.
        /// </summary>
        public Session Session { get; private set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="WebServiceClient"/> class.
        /// </summary>
        /// <param name="folders">The folders controller.</param>
        /// <param name="notes">The notes controller.</param>
        /// <param name="users">The users controller.</param>
        /// <param name="passwordResets">THe password resets controller.</param>
        /// <param name="session">The session.</param>
        public WebServiceClient(IFoldersController folders, INotesController notes, IUsersController users, IPasswordResetController passwordResets, Session session)
        {
            this.Folders = folders;
            this.Notes = notes;
            this.Users = users;
            this.PasswordResets = passwordResets;
            this.Session = session;
        }
        #endregion
    }
}
