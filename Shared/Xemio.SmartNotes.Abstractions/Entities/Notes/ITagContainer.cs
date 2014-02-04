using System.Collections.Generic;

namespace Xemio.SmartNotes.Abstractions.Entities.Notes
{
    /// <summary>
    /// Marks entities that contain tags.
    /// </summary>
    public interface ITagContainer
    {
        /// <summary>
        /// Gets or sets the tags.
        /// </summary>
        ICollection<string> Tags { get; set; }
        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        string UserId { get; set; }
    }
}
