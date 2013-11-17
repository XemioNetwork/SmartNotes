using System.Net.Mail;
using Xemio.SmartNotes.Server.Abstractions.Services;

namespace Xemio.SmartNotes.Server.Infrastructure.Implementations.Services
{
    public class EmailValidationService : IEmailValidationService
    {
        #region Implementation of IEmailValidationService
        /// <summary>
        /// Determines whether the specified <paramref name="email" /> is a valid email address.
        /// </summary>
        /// <param name="email"></param>
        public bool IsValidEmailAddress(string email)
        {
            try
            {
                new MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion
    }
}
