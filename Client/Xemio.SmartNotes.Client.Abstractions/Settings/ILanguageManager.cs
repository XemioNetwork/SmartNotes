﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xemio.SmartNotes.Models.Entities.Users;

namespace Xemio.SmartNotes.Client.Abstractions.Settings
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
