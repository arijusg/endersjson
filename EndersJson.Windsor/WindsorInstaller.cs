using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using EndersJson.Interfaces;

namespace EndersJson.Windsor
{
    public class WindsorInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IJsonService>().ImplementedBy<JsonService>().LifeStyle.Transient);
        }
    }
}