﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using EndersJson.Tests.Framework;
using NUnit.Framework;

namespace EndersJson.Tests.Performance
{
    [TestFixture]
    public class JsonServiceTests : WebApiTestBase
    {
        [SetUp]
        public void SetUp()
        {
            json = new JsonService();
            wait = new ManualResetEvent(false);
        }

        private JsonService json;
        private ManualResetEvent wait;
        private readonly int MaxCount = 100;
        private int CurrentCount;

        [Test]
        public void Should_not_deadlock_under_load()
        {
            for (var threadCount = 0; threadCount < MaxCount; threadCount++)
            {
                Task.Factory.StartNew(async () =>
                {
                    var result = await json.Get<IEnumerable<Person>>(FormatUri("api/persons"));
                    Assert.That(result.Count(), Is.EqualTo(3));
                    Assert.That(json.GetLastStatusCode(), Is.EqualTo(HttpStatusCode.OK));
                    Interlocked.Increment(ref CurrentCount);
                    Console.Write(".");
                    if (CurrentCount >= MaxCount)
                    {
                        Console.WriteLine("Signalling complete");
                        wait.Set();
                    }
                });
                
            }

            wait.WaitOne();

            Console.WriteLine("Complete");
        }
    }
}