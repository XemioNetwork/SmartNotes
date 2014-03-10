﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xemio.SmartNotes.Shared.Entities.Users;

namespace Xemio.SmartNotes.Shared.Entities.Notes
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
            this.CreatedDate = DateTimeOffset.Now;
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
        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        public DateTimeOffset CreatedDate { get; set; }

        #region Methods
        /// <summary>
        /// Creates a deep clone from this instance.
        /// </summary>
        public Folder DeepClone()
        {
            return new Folder
            {
                Id = this.Id,
                Name = this.Name,
                Tags = this.Tags.ToList(),
                UserId = this.UserId,
                ParentFolderId = this.ParentFolderId,
                CreatedDate = this.CreatedDate
            };
        }
        #endregion
    }
}
