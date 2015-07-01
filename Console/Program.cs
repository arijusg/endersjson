using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cachely.Console
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            long isComplete = 0;
            var random = new Random();
            var wait = new ManualResetEvent(false);

            var readCounter = 0;
            var createCounter = 0;
            var deleteCounter = 0;

            var cache = new Cache<Guid>();

            // Reader
            var reader = Task.Factory.StartNew(() =>
            {
                while (Interlocked.Read(ref isComplete) == 0)
                {
                    readCounter++;
                    foreach (var p in cache)
                        System.Console.Write(".");
                    Thread.Sleep(random.Next(5));
                }
            });

            // Creator
            var creator = Task.Factory.StartNew(() =>
            {
                while (Interlocked.Read(ref isComplete) == 0)
                {
                    createCounter++;
                    var newGuid = Guid.NewGuid();
                    cache.SetItem(newGuid.ToString("N"), newGuid);
                    System.Console.Write("+");
                    Thread.Sleep(random.Next(5));
                }
            });

            // Deleter
            var deleter = Task.Factory.StartNew(() =>
            {
                while (Interlocked.Read(ref isComplete) == 0)
                {
                    if (cache.Count > 0)
                    {
                        deleteCounter++;
                        var key = cache.AllKeys.First();
                        cache.ExpireItem(key);
                        System.Console.Write("-");
                    }
                    Thread.Sleep(random.Next(5));
                }
            });

            // Monitor
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(5000);
                Interlocked.Exchange(ref isComplete, 1);
                wait.Set();
            });

            wait.WaitOne();

            // Errors
            if (reader.Exception != null)
                Debug.Fail("\r\nThe basket is no longer thread safe -> {0}".FormatWith(reader.Exception));

            if (creator.Exception != null)
                Debug.Fail("\r\nThe basket is no longer thread safe -> {0}".FormatWith(creator.Exception));

            if (deleter.Exception != null)
                Debug.Fail("\r\nThe basket is no longer thread safe -> {0}".FormatWith(deleter.Exception));

            Thread.Sleep(500);

            System.Console.WriteLine("Reads: {0}, Creates: {1}, Deletes: {2} in 5 second(s).".FormatWith(readCounter, createCounter, deleteCounter));
            System.Console.ReadLine();
        }
    }

    public static class StringExtensions
    {
        public static bool EqualsIgnoreCase(this string value, string compareTo)
        {
            if (value == null && compareTo != null)
                return false;

            if (value != null && compareTo == null)
                return false;

            if (value == null && compareTo == null)
                return true;

            return value.ToLower() == compareTo.ToLower();
        }

        public static string FormatWith(this string format, params object[] args)
        {
            return string.Format(format, args);
        }
    }
}

