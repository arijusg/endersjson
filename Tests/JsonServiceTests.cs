using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using EndersJson.Tests.Framework;
using NUnit.Framework;

namespace EndersJson.Tests
{
    [TestFixture]
    public class JsonServiceTests : WebApiTestBase
    {
        private JsonService json;

        [SetUp]
        public void SetUp()
        {
            json = new JsonService();
        }

        [Test]
        public void Should_be_able_to_GET_resource_from_web_api()
        {
            var result = json.Get<IEnumerable<Person>>(FormatUri("api/persons"));

            Assert.That(result.Count(), Is.EqualTo(3));
            Assert.That(json.GetLastStatusCode(), Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public void Should_be_able_to_GET_resource_from_web_api_using_authorisation_header()
        {
            json.SetHeader("Authorization", Guid.NewGuid().ToString("N"));
            
            var result = json.Get<IEnumerable<Person>>(FormatUri("api/persons"));

            Assert.That(result.Count(), Is.EqualTo(3));
            Assert.That(json.GetLastStatusCode(), Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public void Should_be_able_to_GET_resource_from_web_api_and_handle_not_found()
        {
            json.Get<IEnumerable<Person>>(FormatUri("api/persons_404"));

            Assert.That(json.GetLastStatusCode(), Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public void Should_be_able_to_POST_resource_from_web_api()
        {
            var result = json.Post<Person>(FormatUri("api/person"), Person.Any);

            Assert.That(result, Is.Not.Null);
            Assert.That(json.GetLastStatusCode(), Is.EqualTo(HttpStatusCode.Created));
        }

        [Test]
        public void Should_be_able_to_POST_resource_from_web_api_and_handle_not_found()
        {
            json.Post<Person>(FormatUri("api/person_404"), Person.Any);

            Assert.That(json.GetLastStatusCode(), Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public void Should_be_able_to_PUT_resource_from_web_api()
        {
            json.Put<Person>(FormatUri("api/person"), Person.Any);

            Assert.That(json.GetLastStatusCode(), Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public void Should_be_able_to_PUT_resource_from_web_api_and_handle_not_found()
        {
            json.Put<Person>(FormatUri("api/person_404"), Person.Any);

            Assert.That(json.GetLastStatusCode(), Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public void Should_be_able_to_DELETE_resource_from_web_api()
        {
            json.Delete<Person>(FormatUri("api/person/1"));

            Assert.That(json.GetLastStatusCode(), Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public void Should_be_able_to_DELETE_resource_from_web_api_and_handle_not_found()
        {
            json.Delete<Person>(FormatUri("api/person_404/1"));

            Assert.That(json.GetLastStatusCode(), Is.EqualTo(HttpStatusCode.NotFound));
        }
    }
}