using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Xemio.SmartNotes.Entities.Users;

namespace Xemio.SmartNotes.Entities.Notes
{
    /// <summary>
    /// Contains information of a <see cref="Note" />.
    /// </summary>
    public class Note : AggregateRoot, ITagContainer, IUserSpecificEntity
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Note"/> class.
        /// </summary>
        public Note()
        {
            this.Tags = new Collection<string>();
        }
        #endregion

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the markdown content.
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// Gets or sets the tags.
        /// </summary>
        public Collection<string> Tags { get; set; }
        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// Gets or sets the folder id.
        /// </summary>
        public string FolderId { get; set; }
    }
}
