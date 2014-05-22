using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Xemio.SmartNotes.Server.Abstractions.Mailing;
using Xemio.SmartNotes.Server.Infrastructure.Implementations.Mailing;
using Xemio.SmartNotes.Shared.Entities.Mailing;

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
                    Component.For<IEmailSender>().ImplementedBy<SmtpEmailSender>().LifestyleSingleton().DependsOn(new
                                                                                                                      {
                                                                                                                          host = "localhost",
                                                                                                                          port = 25,
                                                                                                                          username = string.Empty,
                                                                                                                          password = string.Empty,
                                                                                                                          sender = new EmailPerson
                                                                                                                          {
                                                                                                                              Name = "Xemio Notes",
                                                                                                                              Address = "info@xemio-notes.net"
                                                                                                                          }
                                                                                                                      }),

                    Component.For<IEmailFactory>()
                             .ImplementedBy<EmailFactory>()
                             .LifestyleSingleton()
            );
        }
        #endregion
    }
}
