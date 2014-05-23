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
        /// Gets the tags.
        /// </summary>
        /// <param name="text">The text.</param>
        public static string[] GetTags(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return new string[0];

            return 
                (
                    from tag in text.Split(new[] {',', ' '}, StringSplitOptions.RemoveEmptyEntries)
                    select tag.Trim()
                ).ToArray();
        }
        /// <summary>
        /// Makes the first character lower case.
        /// </summary>
        /// <param name="text">The text.</param>
        public static string MakeFirstCharLowerCase(this string text)
        {
            return text.Substring(0, 1).ToLower() + text.Substring(1, text.Length - 1);
        }
        /// <summary>
        /// Strips the HTML tags.
        /// </summary>
        /// <param name="text">The text.</param>
        public static string StripHtmlTags(this string text)
        {
            var array = new char[text.Length];
            int arrayIndex = 0;

            bool inside = false;

            foreach (char c in text)
            {
                if (c == '<')
                {
                    inside = true;
                    continue;
                }

                if (c == '>')
                {
                    inside = false;
                    continue;
                }

                if (!inside)
                {
                    array[arrayIndex] = c;
                    arrayIndex++;
                }
            }
            return new string(array, 0, arrayIndex);
        }
        /// <summary>
        /// Removes all double linebreaks from the text.
        /// </summary>
        /// <param name="text">The text.</param>
        public static string RemoveDoubleBreaks(this string text)
        {
            var result = new List<char>();
            bool foundNewLine = false;

            foreach (char c in text)
            {
                if (c == '\r' || c == '\n')
                {
                    foundNewLine = true;
                    continue;
                }

                if (foundNewLine)
                { 
                    result.Add('\r');
                    result.Add('\n');

                    foundNewLine = false;
                }

                result.Add(c);
            }

            return new string(result.ToArray(), 0, result.Count);
        }
    }
}
