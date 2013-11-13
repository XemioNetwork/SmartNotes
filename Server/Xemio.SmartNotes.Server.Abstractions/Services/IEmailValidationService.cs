namespace Xemio.SmartNotes.Server.Abstractions.Services
{
    /// <summary>
    /// Provides a methods to validate an email address.
    /// </summary>
    public interface IEmailValidationService : IService
    {
        /// <summary>
        /// Determines whether the specified <paramref name="email"/> is a valid email address.
        /// </summary>
        bool IsValidEmailAddress(string email);
    }
}
