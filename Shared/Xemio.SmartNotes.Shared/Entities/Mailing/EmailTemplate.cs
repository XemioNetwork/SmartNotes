using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xemio.SmartNotes.Shared.Entities.Mailing
{
    public class EmailTemplate : AggregateRoot
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="EmailTemplate"/> class.
        /// </summary>
        public EmailTemplate()
        {
            this.LanguageMappings = new Dictionary<string, SubjectAndBody>();
            this.Attachments = new Collection<EmailAttachment>();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the language to subject mapping.
        /// </summary>
        public Dictionary<string, SubjectAndBody> LanguageMappings { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the email should be sent immediately.
        /// </summary>
        public bool SendImmediate { get; set; }

        /// <summary>
        /// Gets or sets the attachments.
        /// </summary>
        public ICollection<EmailAttachment> Attachments { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Returns the email subject for the specified <paramref name="language"/>.
        /// </summary>
        /// <param name="language">The language.</param>
        public string GetEmailSubject(string language)
        {
            return this.LanguageMappings[language].Subject;
        }
        /// <summary>
        /// Returns the email body for the specified <paramref name="language"/>.
        /// </summary>
        /// <param name="language">The language.</param>
        public EmailBody GetEmailBody(string language)
        {
            return this.LanguageMappings[language].Body;
        }
        #endregion
    }
}