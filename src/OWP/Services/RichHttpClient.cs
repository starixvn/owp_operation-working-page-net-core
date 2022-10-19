namespace OWP.Services
{
    using Newtonsoft.Json;
    using System;
    using System.IO;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;

    public interface RichHttpClient
    {
        Task<T> Get<T>(string requestUri) where T : class;

        Task<T> Post<T>(string requestUri, object postData) where T : class;

        Task<Stream> Download(string requestUri);
    }

    public class RichHttpClientException : Exception
    {
        public RichHttpClientException() : base() { }

        public RichHttpClientException(string message) : base(message) { }

        public RichHttpClientException(string message, Exception e) : base(message, e) { }
    }

    public class RichHttpClientImp : RichHttpClient
    {

        private static readonly HttpClient JsonHttpClient = CreateJsonHttpClient();

        private static readonly HttpClient OctetStreamHttpClient = CreateOctetStreamHttpClient();

        private static HttpClient CreateJsonHttpClient()
        {
            return CreateHttpClient("application/json");
        }

        private static HttpClient CreateOctetStreamHttpClient()
        {
            return CreateHttpClient("application/octet-stream");
        }

        private static HttpClient CreateHttpClient(string contentType)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
            client.DefaultRequestHeaders.Add("User-Agent", ".NET HttpClient");

            return client;
        }

        public async Task<T> Get<T>(string requestUri) where T : class
        {
            using (var response = await JsonHttpClient.GetAsync(new Uri(requestUri)).ConfigureAwait(false))
            {
                return await ReadObjectFromResponse<T>(response).ConfigureAwait(false);
            }
        }

        public async Task<T> Post<T>(string requestUri, object postData) where T : class
        {
            var jsonRequest = JsonConvert.SerializeObject(postData);
            using (var stringContent = new StringContent(jsonRequest, Encoding.UTF8, "application/json"))
            {
                using (var response = await JsonHttpClient.PostAsync(new Uri(requestUri), stringContent).ConfigureAwait(false))
                {
                    return await ReadObjectFromResponse<T>(response).ConfigureAwait(false);
                }
            }
        }

        private static async Task<T> ReadObjectFromResponse<T>(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception e)
            {
                var message = $"Exception when deserialize {json} to {typeof(T)}";
                throw new RichHttpClientException(message, e);
            }
        }

        public async Task<Stream> Download(string requestUri)
        {
            return await OctetStreamHttpClient.GetStreamAsync(new Uri(requestUri)).ConfigureAwait(false);
        }
    }
}
