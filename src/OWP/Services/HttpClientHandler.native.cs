using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace OWP.Services
{
    public interface IHttpHandler
    {
        Task<HttpResponseMessage> GetAsync(string requestLink);
    }

    public class HttpClientHandler : IHttpHandler, IDisposable
    {
        private readonly HttpClient client;

        private const int TIME_OUT = 30;

        public HttpClientHandler()
        {
            client = new HttpClient { Timeout = TimeSpan.FromMinutes(TIME_OUT) };
        }

        public async Task<HttpResponseMessage> GetAsync(string requestLink)
        {
            return await client.GetAsync(new Uri(requestLink));
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                client?.Dispose();
            }
        }
    }
}
