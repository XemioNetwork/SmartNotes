using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;
using Xemio.SmartNotes.Entities.Notes;

namespace Xemio.SmartNotes.Infrastructure.Raven.Indexes
{
    /// <summary>
    /// Enables querying for a tag overview.
    /// </summary>
    public class TagsByCount : AbstractMultiMapIndexCreationTask<TagsByCount.Result>
    {
        /// <summary>
        /// The result of the <see cref="TagsByCount"/> index.
        /// </summary>
        public class Result
        {
            public string Tag { get; set; }
            public int Count { get; set; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TagsByCount"/> class.
        /// </summary>
        public TagsByCount()
        {
            this.AddMapForAll<ITagContainer>(containers => from container in containers
                                                           from tag in container.Tags
                                                           select new
                                                                       {
                                                                           Tag = tag.ToLower(),
                                                                           Count = 1
                                                                       });
            this.Reduce = results => from result in results
                                     group result by result.Tag
                                     into g
                                     select new
                                                {
                                                    Tag = g.Key,
                                                    Count = g.Sum(f => f.Count)
                                                };


            this.Sort(f => f.Count, SortOptions.Int);
        }

        /// <summary>
        /// Gets the name of the index.
        /// </summary>
        public override string IndexName
        {
            get { return "Tags/ByCount"; }
        }
    }
}
