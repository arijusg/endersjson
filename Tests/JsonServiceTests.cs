using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using EndersJson.Tests.Framework;
using NUnit.Framework;

namespace EndersJson.Tests
{
    [TestFixture]
    public class JsonServiceTests : WebApiTestBase
    {
        [SetUp]
        public void SetUp()
        {
            json = new JsonService();
        }

        private JsonService json;

        [Test]
        public async Task Should_be_able_to_DELETE_resource_from_web_api()
        {
            await json.Delete<Person>(FormatUri("api/person/1"));
        }

        [Test]
        [ExpectedException(typeof (HttpRequestException))]
        public async Task Should_be_able_to_DELETE_resource_from_web_api_and_throw_if_not_found()
        {
            await json.Delete<Person>(FormatUri("api/person_404/1"));
        }

        [Test]
        public async Task Should_be_able_to_GET_resource_from_web_api()
        {
            var result = await json.Get<IEnumerable<Person>>(FormatUri("api/persons"));
            Assert.That(result.Count(), Is.EqualTo(3));
        }

        [Test]
        [ExpectedException(typeof (HttpRequestException))]
        public async Task Should_be_able_to_GET_resource_from_web_api_and_throw_if_not_found()
        {
            await json.Get<IEnumerable<Person>>(FormatUri("api/persons_404"));
        }

        [Test]
        public async Task Should_be_able_to_GET_resource_from_web_api_using_authorisation_header()
        {
            json.SetHeader("Authorization", Guid.NewGuid().ToString("N"));
            var result = await json.Get<IEnumerable<Person>>(FormatUri("api/persons"));
            Assert.That(result.Count(), Is.EqualTo(3));
        }

        [Test]
        public async Task Should_be_able_to_POST_resource_from_web_api()
        {
            var result = await json.Post<Person>(FormatUri("api/person"), Person.Any);
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [ExpectedException(typeof (HttpRequestException))]
        public async Task Should_be_able_to_POST_resource_from_web_api_and_throw_if_not_found()
        {
            await json.Post<Person>(FormatUri("api/person_404"), Person.Any);
        }

        [Test]
        public async Task Should_be_able_to_PUT_resource_from_web_api()
        {
            await json.Put<Person>(FormatUri("api/person"), Person.Any);
        }

        [Test]
        [ExpectedException(typeof (HttpRequestException))]
        public async Task Should_be_able_to_PUT_resource_from_web_api_and_throw_if_not_found()
        {
            await json.Put<Person>(FormatUri("api/person_404"), Person.Any);
        }
    }
}