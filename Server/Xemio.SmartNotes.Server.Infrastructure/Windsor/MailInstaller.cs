using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Newtonsoft.Json.Converters;
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
            this.InstallEmailService(container);

            container.Register
            (

                Component.For<IEmailFactory>()
                            .ImplementedBy<EmailFactory>()
                            .LifestyleSingleton()
                            .DependsOn(
                                Dependency.OnValue("sender", new EmailPerson
                                {
                                    Name = Dependency.OnAppSettingsValue("XemioNotes/EmailSenderName").Value,
                                    Address = Dependency.OnAppSettingsValue("XemioNotes/EmailSenderAddress").Value
                                }))
            );
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Installs the email service configured in "XemioNotes/EmailService".
        /// </summary>
        /// <param name="container">The container.</param>
        /// <exception cref="System.InvalidOperationException"></exception>
        private  void InstallEmailService(IWindsorContainer container)
        {
            string emailService = Dependency.OnAppSettingsValue("XemioNotes/EmailService").Value;
            if (emailService == "Smtp")
            {
                container.Register
                    (
                        Component.For<IEmailSender>()
                            .ImplementedBy<SmtpEmailSender>()
                            .LifestyleSingleton()
                            .DependsOn(
                                Dependency.OnAppSettingsValue("host", "XemioNotes/SmtpHost"),
                                Dependency.OnAppSettingsValue("port", "XemioNotes/SmtpPort"),
                                Dependency.OnAppSettingsValue("username", "XemioNotes/SmtpUsername"),
                                Dependency.OnAppSettingsValue("password", "XemioNotes/SmtpPassword"))
                    );
            }
            else if (emailService == "MailGun")
            {
                container.Register
                    (
                        Component.For<IEmailSender>()
                            .ImplementedBy<MailGunEmailSender>()
                            .LifestyleSingleton()
                            .DependsOn(
                                Dependency.OnAppSettingsValue("apiKey", "XemioNotes/MailGunApiKey"),
                                Dependency.OnAppSettingsValue("customDomain", "XemioNotes/MailGunCustomDomain"))
                    );
            }
            else
            {
                throw new InvalidOperationException(
                    string.Format("Can't find the email service for key '{0}'. Use 'Smtp' or 'MailGun'.", emailService));
            }
        }

        #endregion
    }
}
