using EndersJson.Extensions;
using NUnit.Framework;

namespace EndersJson.Tests.Extensions
{
    [TestFixture]
    public class HttpExtensionsTests
    {
        [SetUp]
        public void SetUp()
        {
        }

        [Test]
        public void Should_be_able_to_convert_anonymous_object_to_query_string()
        {
            var anyInstance = new {Id = 9, Name = "Foo", Age = 21};

            var queryString = anyInstance.ToQueryString();

            Assert.That(queryString, Is.EqualTo("Age=21&Id=9&Name=Foo"));
        }
    }
}