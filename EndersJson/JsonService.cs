using System.Collections.Concurrent;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using EndersJson.Extensions;
using EndersJson.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace EndersJson
{
    public class JsonService : IJsonService
    {
        private readonly HttpClient client;
        private readonly JsonSerializerSettings settings;
        private static ConcurrentDictionary<string, string> headers;
        private HttpStatusCode lastStatusCode = HttpStatusCode.OK;

        static JsonService()
        {
            headers = new ConcurrentDictionary<string, string>();
        }

        public JsonService()
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
        }

        public async Task<T> Get<T>(string uri)
        {
            var requestMessage = CreateRequest(HttpMethod.Get, uri);
            var result = await client.SendAsync(requestMessage);
            lastStatusCode = result.StatusCode;
            if (!result.IsSuccessStatusCode)
                return default(T);
            return DeserialiseResponse<T>(result);
        }

        public async Task<T> Get<T>(string uri, object data)
        {
            var fullUri = string.Format("{0}?{1}", uri, data.ToQueryString());
            var requestMessage = CreateRequest(HttpMethod.Get, fullUri);
            var result = await client.SendAsync(requestMessage);
            lastStatusCode = result.StatusCode;
            if (!result.IsSuccessStatusCode)
                return default(T);
            return DeserialiseResponse<T>(result);
        }

        public async Task<T> Post<T>(string uri)
        {
            var request = CreateRequest(HttpMethod.Post, uri);
            request.Content = SerializeRequest();
            var result = await client.SendAsync(request);
            lastStatusCode = result.StatusCode;
            if (!result.IsSuccessStatusCode)
                return default(T);
            return DeserialiseResponse<T>(result);
        }

        public async Task<T> Post<T>(string uri, object data)
        {
            var request = CreateRequest(HttpMethod.Post, uri);
            request.Content = SerializeRequest(data);
            var result = await client.SendAsync(request);
            lastStatusCode = result.StatusCode;
            if (!result.IsSuccessStatusCode)
                return default(T);
            return DeserialiseResponse<T>(result);
        }

        public async Task<T> Put<T>(string uri)
        {
            var request = CreateRequest(HttpMethod.Put, uri);
            request.Content = SerializeRequest();
            var result = await client.SendAsync(request);
            lastStatusCode = result.StatusCode;
            if (!result.IsSuccessStatusCode)
                return default(T);
            return DeserialiseResponse<T>(result);
        }

        public async Task<T> Put<T>(string uri, object data)
        {
            var request = CreateRequest(HttpMethod.Put, uri);
            request.Content = SerializeRequest(data);
            var result = await client.SendAsync(request);
            lastStatusCode = result.StatusCode;
            if (!result.IsSuccessStatusCode)
                return default(T);
            return DeserialiseResponse<T>(result);
        }

        public async Task<T> Delete<T>(string uri)
        {
            var request = CreateRequest(HttpMethod.Delete, uri);
            request.Content = SerializeRequest();
            var result = await client.SendAsync(request);
            lastStatusCode = result.StatusCode;
            if (!result.IsSuccessStatusCode)
                return default(T);
            return DeserialiseResponse<T>(result);
        }

        public HttpStatusCode GetLastStatusCode()
        {
            return lastStatusCode;
        }

        public void SetHeader(string header, string value)
        {
            headers[header] = value;
        }

        public void ClearHeader(string header)
        {
            headers[header] = null;
        }

        public void ClearHeaders()
        {
            headers.Clear();
        }

        private HttpContent SerializeRequest(object data = null)
        {
            StringContent request;
            
            if (data == null) 
                request = new StringContent(string.Empty);
            else
                request = new StringContent(JsonConvert.SerializeObject(data, settings));

            request.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return request;
        }

        private HttpRequestMessage CreateRequest(HttpMethod method, string uri)
        {
            var requestMessage = new HttpRequestMessage(method, uri);
            requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            foreach (var headerValue in headers.Where(xy => !string.IsNullOrEmpty(xy.Value)))
                requestMessage.Headers.Add(headerValue.Key, headerValue.Value);
            return requestMessage;
        }

        private T DeserialiseResponse<T>(HttpResponseMessage result)
        {
            var payload = result.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<T>(payload, settings);
        }

        public void Dispose()
        {
            client.Dispose();
        }
    }
}