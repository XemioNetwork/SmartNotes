namespace Xemio.SmartNotes.Abstractions.Entities
{
    /// <summary>
    /// A base class for all <see cref="AggregateRoot"/>s.
    /// </summary>
    public abstract class AggregateRoot
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public string Id { get; set; }
    }
}
