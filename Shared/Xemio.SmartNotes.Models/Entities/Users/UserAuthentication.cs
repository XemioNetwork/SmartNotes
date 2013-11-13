namespace Xemio.SmartNotes.Models.Entities.Users
{
    /// <summary>
    /// Contains authentication informations for a <see cref="User"/>.
    /// </summary>
    public class UserAuthentication
    {
        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// Gets or sets the password hash.
        /// </summary>
        public byte[] AuthorizationHash { get; set; }
    }
}
