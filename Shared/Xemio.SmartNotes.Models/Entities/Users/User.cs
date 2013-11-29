namespace Xemio.SmartNotes.Models.Entities.Users
{
    /// <summary>
    /// Contains information about a single <see cref="User" />.
    /// </summary>
    public class User : AggregateRoot
    {
        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        public string EmailAddress { get; set; }
    }
}
