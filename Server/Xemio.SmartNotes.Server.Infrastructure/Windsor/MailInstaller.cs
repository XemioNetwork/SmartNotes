using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Xemio.SmartNotes.Server.Abstractions.Email;
using Xemio.SmartNotes.Server.Infrastructure.Implementations.Email;

namespace Xemio.SmartNotes.Server.Infrastructure.Windsor
{
    /// <summary>
    /// Installs the <see cref="IEmailSender"/>.
    /// </summary>
    public class MailInstaller : IWindsorInstaller
    {
        #region Implementation of IWindsorInstaller
        /// <summary>
        /// Performs the installation in the <see cref="T:Castle.Windsor.IWindsorContainer" />.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="store">The configuration store.</param>
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register
            (
                Component.For<IEmailSender>().ImplementedBy<SmtpEmailSender>().DependsOn(new
                                                                                         {
                                                                                             host = "mail.xemio.net",
                                                                                             port = 587,
                                                                                             username = "notes@xemio.net",
                                                                                             password = "N0t3SSend!er",
                                                                                         }),
                Component.For<IEmailFactory>().ImplementedBy<SmtpEmailFactory>().DependsOn(new
                                                                                           {
                                                                                               senderEmailAddress = "notes@xemio.net",
                                                                                               senderName = "Xemio Notes"
                                                                                           })
            );
        }
        #endregion
    }
}
