using System.Threading.Tasks;
using Xemio.SmartNotes.Shared.Entities.Mailing;

namespace Xemio.SmartNotes.Server.Abstractions.Mailing
{
    public interface IEmailSender
    {
        /// <summary>
        /// Sends the specified <see cref="email"/>.
        /// </summary>
        /// <param name="email">The mail.</param>
        Task SendAsync(SentEmail email);
    }
}
