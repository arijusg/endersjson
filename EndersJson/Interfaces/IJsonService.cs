using System;
using System.Net;

namespace EndersJson.Interfaces
{
    public interface IJsonService : IDisposable
    {
        T Get<T>(string uri);
        T Get<T>(string uri, object data);
        T Post<T>(string uri);
        T Post<T>(string uri, object data);
        T Put<T>(string uri);
        T Put<T>(string uri, object data);
        T Delete<T>(string uri);
        HttpStatusCode GetLastStatusCode();
        void SetHeader(string header, string value);
        void ClearHeader(string header);
        void ClearHeaders();
    }
}