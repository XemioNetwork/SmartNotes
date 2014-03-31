using System;

namespace Xemio.SmartNotes.Shared.Entities.Users
{
    /// <summary>
    /// Contains information about a single <see cref="User" />.
    /// </summary>
    public class User : AggregateRoot
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        public User()
        {
            this.PreferredLanguage = "en-EN";
        }
        #endregion

        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        public string EmailAddress { get; set; }
        /// <summary>
        /// Gets or sets the preferred language.
        /// </summary>
        public string PreferredLanguage { get; set; }
        /// <summary>
        /// Gets or sets the time zone identifier (IANA TimeZones).
        /// </summary>
        public string TimeZoneId { get; set; }
    }
}
