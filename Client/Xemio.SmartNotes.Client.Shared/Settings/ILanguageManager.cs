using System.Globalization;
using Xemio.SmartNotes.Shared.Entities.Users;

namespace Xemio.SmartNotes.Client.Shared.Settings
{
    public interface ILanguageManager
    {
        /// <summary>
        /// Gets or sets the current language.
        /// </summary>
        CultureInfo CurrentLanguage { get; set; }
        /// <summary>
        /// Sets the language from the given <paramref name="user"/>.
        /// </summary>
        /// <param name="user">The user.</param>
        void SetLanguageFromUser(User user);
    }
}
