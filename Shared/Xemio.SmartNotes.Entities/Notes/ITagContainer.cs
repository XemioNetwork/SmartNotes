using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Xemio.SmartNotes.Entities.Notes
{
    /// <summary>
    /// Marks entities that contain tags.
    /// </summary>
    public interface ITagContainer
    {
        /// <summary>
        /// Gets or sets the tags.
        /// </summary>
        Collection<string> Tags { get; set; } 
    }
}
