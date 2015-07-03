using System;
using System.Net;
using System.Threading.Tasks;

namespace EndersJson.Interfaces
{
    public interface IJsonService : IDisposable
    {
        Task<T> Get<T>(string uri); 
        Task<T> Get<T>(string uri, object data);
        Task<T> Post<T>(string uri);
        Task<T> Post<T>(string uri, object data);
        Task<T> Put<T>(string uri);
        Task<T> Put<T>(string uri, object data);
        Task<T> Delete<T>(string uri);
        HttpStatusCode GetLastStatusCode();
        void SetHeader(string header, string value);
        void ClearHeader(string header);
        void ClearHeaders();
    }
}