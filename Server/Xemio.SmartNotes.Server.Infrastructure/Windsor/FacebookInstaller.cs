using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Xemio.SmartNotes.Server.Abstractions.Social;
using Xemio.SmartNotes.Server.Infrastructure.Implementations.Social;

namespace Xemio.SmartNotes.Server.Infrastructure.Windsor
{
    public class FacebookInstaller : IWindsorInstaller
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
                Component.For<IFacebookService>()
                    .ImplementedBy<FacebookService>()
                    .LifestyleTransient()
                    .DependsOn(
                        Dependency.OnAppSettingsValue("appId", "XemioNotes/FacebookAppId"),
                        Dependency.OnAppSettingsValue("appSecret", "XemioNotes/FacebookAppSecret"))
            );
        }
        #endregion
    }
}
