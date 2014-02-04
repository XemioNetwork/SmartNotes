namespace Xemio.SmartNotes.Shared.Entities.Users
{
    /// <summary>
    /// Marks entities that are specific for a single <see cref="User"/>.
    /// </summary>
    public interface IUserSpecificEntity
    {
        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        string UserId { get; set; }
    }
}
