using System.Linq;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;
using Xemio.SmartNotes.Shared.Entities.Notes;
using Xemio.SmartNotes.Shared.Models;

namespace Xemio.SmartNotes.Server.Infrastructure.RavenDB.Indexes
{
    /// <summary>
    /// Enables querying for a tag overview.
    /// </summary>
    public class TagsByCount : AbstractMultiMapIndexCreationTask<Tag>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TagsByCount"/> class.
        /// </summary>
        public TagsByCount()
        {
            this.AddMapForAll<ITagContainer>(containers => from container in containers
                                                           from tag in container.Tags
                                                           select new
                                                                       {
                                                                           container.UserId,
                                                                           Name = tag.ToLower(),
                                                                           Count = 1
                                                                       });
            this.Reduce = results => from result in results
                                     group result by new { result.Name, result.UserId } into g
                                     select new
                                                {
                                                    g.Key.UserId,
                                                    g.Key.Name,
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
