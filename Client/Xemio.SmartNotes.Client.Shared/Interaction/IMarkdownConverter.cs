using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xemio.SmartNotes.Client.Shared.Interaction
{
    public interface IMarkdownConverter
    {
        /// <summary>
        /// Converts the specified markdown to HTML.
        /// </summary>
        /// <param name="markdown">The markdown.</param>
        string Convert(string markdown);
    }
}
