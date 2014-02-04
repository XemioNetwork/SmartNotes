﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xemio.SmartNotes.Abstractions.Entities.Users;

namespace Xemio.SmartNotes.Abstractions.Entities.Notes
{
    /// <summary>
    /// Contains information about a folder.
    /// </summary>
    public class Folder : AggregateRoot, ITagContainer, IUserSpecificEntity
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Folder"/> class.
        /// </summary>
        public Folder()
        {
            this.Tags = new Collection<string>();
        }
        #endregion

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the tags.
        /// </summary>
        public ICollection<string> Tags { get; set; }
        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// Gets or sets the parent folder identifier.
        /// </summary>
        public string ParentFolderId { get; set; }
    }
}
