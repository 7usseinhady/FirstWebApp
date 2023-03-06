using WebApp.SharedKernel.Consts;
using WebApp.SharedKernel.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace WebApp.SharedKernel.Helpers
{
    public class BaseApiConnection : IBaseApiConnection
    {
        private readonly IHttpClientFactory _clientFactory;
        protected HttpClient _client;

        public BaseApiConnection(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public void SetApiUri(string UriKey)
        {
            _client = _clientFactory.CreateClient(UriKey);
        }

        public async Task<JObject> GetAsync(string controller, string action = null, string id = null, string queryString = null)
        {
            var uri = buildUrl(controller, action, id, queryString);
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            return await SendAsync(request);
        }
        public async Task<JObject> PostAsync(string controller, string action = null, string id = null, string queryString = null, object body = null)
        {
            var uri = buildUrl(controller, action, id, queryString);
            var request = new HttpRequestMessage(HttpMethod.Post, uri);
            request.Content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
            return await SendAsync(request);
        }
        public async Task<JObject> PutAsync(string controller, string action = null, string id = null, string queryString = null, object body = null)
        {
            var uri = buildUrl(controller, action, id, queryString);
            var request = new HttpRequestMessage(HttpMethod.Put, uri);
            request.Content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
            return await SendAsync(request);
        }
        public async Task<JObject> PatchAsync(string controller, string action = null, string id = null, string queryString = null, object body = null)
        {
            var uri = buildUrl(controller, action, id, queryString);
            var request = new HttpRequestMessage(HttpMethod.Patch, uri);
            request.Content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
            return await SendAsync(request);
        }
        public async Task<JObject> DeleteAsync(string controller, string action = null, string id = null, string queryString = null, object body = null)
        {
            var uri = buildUrl(controller, action, id, queryString);
            var request = new HttpRequestMessage(HttpMethod.Delete, uri);
            request.Content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
            return await SendAsync(request);
        }

        protected string buildUrl(string controller, string action = null, string id = null, string queryString = null)
        {
            StringBuilder sb = new StringBuilder($"{controller}");
            sb.Append(!string.IsNullOrWhiteSpace(action) ? $"/{action}" : "");
            sb.Append(!string.IsNullOrWhiteSpace(id) ? $"/{id}" : "");
            //sb.Append($"?culture={CultureInfo.CurrentCulture.Name}");
            sb.Append(!string.IsNullOrWhiteSpace(queryString) ? $"{queryString}" : "");
            return sb.ToString();
        }
        protected async Task<JObject> SendAsync(HttpRequestMessage request)
        {
            HttpResponseMessage response = await _client.SendAsync(request);
            JObject result;
            try
            {
                result = JObject.Parse(await response.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                result = new JObject
                {
                    { Res.state, false },
                    { Res.message, response.ReasonPhrase },
                    { Res.error, ex.Message }
                };
            }

            var statusCode = (int)response.StatusCode;
            if (!result.ContainsKey(Res.state))
            {
                if (statusCode >= 200 && statusCode < 300)
                    result.Add(Res.state, true);
                else
                    result.Add(Res.state, false);
            }
            result.Add(Res.statusCode, statusCode);

            return result;
        }

    }
}
