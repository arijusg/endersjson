using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace EndersJson.Interfaces
{
    public interface IJsonService : IDisposable
    {
        Task<T> Get<T>(string uri, object data = null);
        Task<HttpResponseMessage> Get(string uri, object data = null);
        Task<string> GetString(string uri, object data = null);
        Task<T> Post<T>(string uri, object data = null, bool dontSerialize = false);
        Task<HttpResponseMessage> Post(string uri, object data = null, bool dontSerialize = false);
        Task<T> Put<T>(string uri, object data = null, bool dontSerialize = false);
        Task<HttpResponseMessage> Put(string uri, object data = null, bool dontSerialize = false);
        Task<T> Delete<T>(string uri);
        Task<HttpResponseMessage> Delete(string uri);
        void SetHeader(string header, string value);
        void ClearHeader(string header);
        void ClearHeaders();
        void EnableOnlySuccessOnlyMode(bool successOnly = true);
    }
}