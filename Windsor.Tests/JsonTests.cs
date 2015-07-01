using Castle.Windsor;
using EndersJson.Interfaces;
using EndersJson.Windsor;
using NUnit.Framework;

namespace Cachely.Tests.Windsor
{
    [TestFixture]
    public class JsonTests
    {
        [Test]
        public void Should_resolve_json_client_from_windsor()
        {
            var container = new WindsorContainer();
            container.Install(new WindsorInstaller());
            Assert.That(container.Resolve<IJsonService>(), Is.Not.Null);
        }
    }
}