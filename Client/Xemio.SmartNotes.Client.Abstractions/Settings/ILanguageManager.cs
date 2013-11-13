using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xemio.SmartNotes.Client.Abstractions.Settings
{
    public interface ILanguageManager
    {
        /// <summary>
        /// Gets or sets the current language.
        /// </summary>
        CultureInfo CurrentLanguage { get; set; }
    }
}
