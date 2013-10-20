using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WPFLocalizeExtension.Engine;
using Xemio.CommonLibrary.Storage;
using Xemio.SmartNotes.Client.Data;

namespace Xemio.SmartNotes.Client.Language
{
    public class LanguageManager : ILanguageManager, IDisposable
    {
        #region Constants
        private const string LanguageKey = "Language";
        #endregion

        #region Fields
        private CultureInfo _currentLanguage;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="LanguageManager"/> class.
        /// </summary>
        public LanguageManager()
        {
            using (var storage = new SecureDataStorage())
            {
                string currentLanguage = storage.Retrieve<string>(LanguageKey);
                if (currentLanguage != null)
                {
                    this.CurrentLanguage = new CultureInfo(currentLanguage);
                }
                else
                {
                    this.CurrentLanguage = new CultureInfo("de");
                }
            }
        }
        #endregion

        #region Implementation of ILanguageManager
        /// <summary>
        /// Gets or sets the current language.
        /// </summary>
        public CultureInfo CurrentLanguage
        {
            get { return this._currentLanguage; }
            set
            {
                if (value == null)
                    return;

                if (value.Equals(this._currentLanguage) == false)
                {
                    this._currentLanguage = value;

                    LocalizeDictionary.Instance.Culture = value;
                    Thread.CurrentThread.CurrentCulture = value;
                    Thread.CurrentThread.CurrentUICulture = value;
                }
            }
        }
        #endregion

        #region Implementation of IDisposable
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            using (var storage = new SecureDataStorage())
            {
                storage.Store(this.CurrentLanguage.Name, LanguageKey);
            }
        }
        #endregion
    }
}
