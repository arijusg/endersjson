# endersjson

| Downloads | Version |
|-----------|---------|
| ![NuGet Total](https://img.shields.io/nuget/dt/EndersJson.svg) | ![NuGet Version](https://img.shields.io/nuget/v/EndersJson.svg) |

A easy to use JSON client. If you have ever tried to add headers to an HttpClient then you would know how painful it can be to do 
the most basic of interactions with an endpoint. This client comes DI ready but is asynchronous. There is an additional bolt on if 
you are using FluentWindsor which will handle all the registration for you. 

##How it works

First start by pulling the latest package from NuGet. After doing so you can create your JSON client by simply doing the following:

``` csharp
var client = new JsonService();
```

###Person Example

Let's say we would like to interact with an end point that manages contacts named Person. The example POCO we will use is something
like this: 

``` csharp
public class Person
{
    public int Age { get; set; }
    public string Name { get; set; }
}
```

Not really something you would find out in the wild but at least sets the tone for how we use the client with strongly typed objects
and is referenced in all the examples throughout this doc. 

Lastly let's also assume your base url for the resources you are interacting with are hosted on the following base url:

    http://localhost:9999/

###GET Requests

If we were to make a GET request to find all the persons from the API we would target the following end point.

    http://localhost:9999/api/persons

This returns an `IEnumerable` of `Person`. If we had to issue the request the code would look something like this:

``` csharp
var client = new JsonService();
var result = await client.Get<IEnumerable<Person>>("http://localhost:9999/api/persons");
client.Dispose();
```

You can also pass query variables to a Get overload using an anonymous instance like so:

``` csharp
var client = new JsonService();
var result = await client.Get<IEnumerable<Person>>("http://localhost:9999/api/persons", new { Skip = 10, Take = 20 });
client.Dispose();
```

This will produce the following get url `http://localhost:9999/api/persons?skip=10&take=20`.

###POST Requests

Next let's take a look at how we post(create new objects). Given the following end point which responds to POST requests.

    http://localhost:9999/api/person

Then we would have code that goes something like this:

``` csharp
var person = new Person() { Age = 10, Name = "Johnny" };
var client = new JsonService();
await client.Post<Person>("http://localhost:9999/api/person", person);
client.Dispose();
```

###PUT Requests

To update objects we would opt for a put, let's look at some more code so we can figure out how that works. Again we assume the 
same end point but it has to respond to the correct verb like so.

	http://localhost:9999/api/person

Then we would have code that goes something like this:

``` csharp
var person = new Person() { Age = 10, Name = "Johnny" };
var client = new JsonService();
await client.Put<Person>("http://localhost:9999/api/person", person);
client.Dispose();
```

In this case the code is assuming the same object of type `Person` would be returned by way of the generic type parameter. Arguably a
little superflous if you are not interested in the result. 

###DELETE Requests

You can also make DELETE requests and as always we assume the same endpoint however slightly changed. 

	http://localhost:9999/api/person/{id:int}

The code also assumes a generic for the return type if any. 

``` csharp
var client = new JsonService();
await client.Delete<Person>(FormatUri("api/person/1"));
client.Dispose();
```

##But I dont care about POCO's

So let's say you cannot be asked to roll strongly typed objects like `Person` for your interactions, that is perfectly ok. In that
case I would recommend the use of `dynamic` which works just as well. Let's see how we do this with the GET example. 

``` csharp
var client = new JsonService();
var result = await client.Get<dynamic>("http://localhost:9999/api/persons");
client.Dispose();
```

The only caveat is that you will lose compile time safety with whatever you do with the result. 

##How do I deal with errors?

Glad you asked. The client has provisining for checking the last http status code returned from the end point. Here is how we
check for a 404 not found. 

``` csharp
await client.Put<Person>("http://localhost:9999/api/person_404", new Person());
var httpErrorCode = client.GetLastStatusCode(); // Should be HttpStatusCode.NotFound
```

##What about headers?

You can do this via the `SetHeader` member on the client service like so.

``` csharp
client.SetHeader("Authorization", Guid.NewGuid().ToString("N"));
```

Please bare in mind that this value will be applied to every subsquent the client makes. So if you need to get rid of the header you 
have 2 options. 

``` csharp
client.ClearHeader("Authorization");
```

Or

``` csharp
client.ClearHeaders(); // Which resets everything
```

##Dont synchronise using Task.Result

If you try to synchronise this library by using `Task.Result` it will deadlock (thanks to Tony Das for pointing this out). You cannot synchronise using this method if you are using this library
in a multi-threaded context. You are also killing off scalability and your code and it wont be stable. The really painful part about this is that all of your calling code has to be changed to
use async/await. 

##A Castle Windsor version

There is also a castle windsor version of this cache which can be auto loaded using FluentWindsor. For more about how you can use this please see https://github.com/cryosharp/fluentwindsor.

##Problems?

For any problems please sign into github and raise issues.





	