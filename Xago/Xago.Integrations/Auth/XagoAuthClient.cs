using Newtonsoft.Json;

namespace Xago.Integrations.Auth
{
    public class XagoAuthClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public readonly static string AuthClient = "XagoAuth";

        public XagoAuthClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<XagoAuthResponse> GetToken(HttpRequestMessage request)
        {
            var client = _httpClientFactory.CreateClient(AuthClient);
            var response = await client.SendAsync(request);
            var responseData = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<XagoAuthResponse>(responseData);

        }
    }
}
