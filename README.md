# endersjson

A easy to use JSON client. If you have ever tried to add headers to an HttpClient then you would know how painful it can be to do 
the most basic of interactions with an endpoint. The async nature of the HttpClient also assumes much about your architecture. 
Sometimes this is just over kill and you end up writing heaps of boiler plate code to make things happen. This client comes DI
ready and is synchronous. There is an additional bolt on if you are using FluentWindsor which will handle all the registration 
for you. 

##How it works

First start by pulling the latest package from NuGet. After doing so you can create your JSON client by simply doing the following:

    var client = new JsonService();

###Persons Example

Let's say we would like to interact with an end point that manages contacts named Person. The example POCO we will use is something
like this: 

    public class Person
    {
        public int Age { get; set; }
        public string Name { get; set; }
    }

Not really something you would find out in the wild but at least sets the tone for how we use the client with strongly typed objects
and is referenced in all the examples throughout this doc. 

Lastly let's also assume your base url for the resources you are interacting with are hosted on the following base url:

    http://localhost:9999/

###GET Requests

If we were to make a GET request to find all the persons from the API we would target the following end point.

    http://localhost:9999/api/persons

This returns an `IEnumerable` of `Person`. If we had to issue the request the code would look something like this:

	var client = new JsonService();
	var result = client.Get<IEnumerable<Person>>("http://localhost:9999/api/persons");
	client.Dispose();

You can also pass query variables to a Get overload using an anonymous instance like so:

	var client = new JsonService();
	var result = client.Get<IEnumerable<Person>>("http://localhost:9999/api/persons", new { Skip = 10, Take = 20 });
	client.Dispose();

This will produce the following get url `http://localhost:9999/api/persons?skip=10&take=20`.

###POST Requests

Next let's take a look at how we post(create new objects). Given the following end point which responds to POST requests.

    http://localhost:9999/api/person

Then we would have code that goes something like this:

    var person = new Person() { Age = 10, Name = "Johnny" };
	var client = new JsonService();
	client.Post<Person>("http://localhost:9999/api/person", person);
	client.Dispose();

###PUT Requests

To update objects we would opt for a put, let's look at some more code so we can figure out how that works. Again we assume the 
same end point but it has to respond to the correct verb like so.

	http://localhost:9999/api/person

Then we would have code that goes something like this:

    var person = new Person() { Age = 10, Name = "Johnny" };
	var client = new JsonService();
	client.Put<Person>("http://localhost:9999/api/person", person);
	client.Dispose();

In this case the code is assuming the same object of type `Person` would be returned by way of the generic type parameter. Arguably a
little superflous if you are not interested in the result. 

###DELETE Requests

You can also make DELETE requests and as always we assume the same endpoint however slightly changed. 

	http://localhost:9999/api/person/{id:int}

The code also assumes a generic for the return type if any. 

	json.Delete<Person>(FormatUri("api/person/1"));



