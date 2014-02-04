using System;

namespace Xemio.SmartNotes.Shared.Entities.Notes
{
    public class AttachedFile
    {
        /// <summary>
        /// Gets or sets the id of the attachment.
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }
    }
}