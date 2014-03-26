using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarkdownSharp;
using Xemio.SmartNotes.Client.Shared.Interaction;

namespace Xemio.SmartNotes.Client.Windows.Implementations.Interaction
{
    public class MarkdownConverter : IMarkdownConverter
    {
        #region Fields
        private readonly Markdown _markdown;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="MarkdownConverter"/> class.
        /// </summary>
        public MarkdownConverter()
        {
            this._markdown = new Markdown(new MarkdownOptions
            {
                AutoHyperlink = true,
                LinkEmails = true
            });   
        }
        #endregion

        #region Implementation of IMarkdownConverter
        /// <summary>
        /// Converts the specified markdown to HTML.
        /// </summary>
        /// <param name="markdown">The markdown.</param>
        public string Convert(string markdown)
        {
            return this._markdown.Transform(markdown);
        }
        #endregion
    }
}
