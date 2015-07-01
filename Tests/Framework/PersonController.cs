using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;

namespace EndersJson.Tests.Framework
{
    public class Person
    {
        public static readonly Person Any = new Person() { Name = "Any", Age = 1 };
        public int Age { get; set; }
        public string Name { get; set; }
    }

    [RoutePrefix("api/person")]
    public class PersonController : ApiController
    {
        [HttpPost]
        public IHttpActionResult Post(Person person)
        {
            return Created("api/person/123", person);
        }

        [HttpPut]
        public IHttpActionResult Put(Person person)
        {
            return Ok();
        }

        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult Delete(int id)
        {
            return Ok();
        }
    }

    [RoutePrefix("api/Persons")]
    public class PersonsController : ApiController
    {
        [HttpGet]
        [ResponseType(typeof(IEnumerable<Person>))]
        public IHttpActionResult Get()
        {
            return Ok(new[]
            {
                new Person() {Name = "Jon", Age = 20},
                new Person() {Name = "Jill", Age = 22},
                new Person() {Name = "Jeff", Age = 24},
            });
        }
    }
}