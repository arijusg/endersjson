using System;
using Microsoft.Owin.Hosting;
using NUnit.Framework;

namespace EndersJson.Tests.Framework
{
    public class WebApiTestBase
    {
        private readonly string BaseUri = "http://localhost:9999";
        private IDisposable server;

        [TestFixtureSetUp]
        public virtual void SetUpFixture()
        {
            server = WebApp.Start<Startup>(BaseUri);
        }

        [TestFixtureTearDown]
        public virtual void FixtureDispose()
        {
            server.Dispose();
        }

        public string FormatUri(string relativeUri)
        {
            return string.Format("{0}/{1}", BaseUri, relativeUri);
        }
    }
}