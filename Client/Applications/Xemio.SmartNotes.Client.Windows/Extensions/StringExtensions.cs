using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xemio.SmartNotes.Client.Windows.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Returns the comma separated tags in the specified <paramref name="tagString"/>.
        /// </summary>
        /// <param name="tagString">The tag string.</param>
        public static string[] GetTags(this string tagString)
        {
            if (tagString == null)
                return new string[0];

            return (from tag in tagString.Split(new []{',', ' '}, StringSplitOptions.RemoveEmptyEntries)
                    select tag.Trim()).ToArray();
        }
        /// <summary>
        /// Makes the first character lower case.
        /// </summary>
        /// <param name="text">The text.</param>
        public static string MakeFirstCharLowerCase(this string text)
        {
            return text.Substring(0, 1).ToLower() + text.Substring(1, text.Length - 1);
        }
    }
}
