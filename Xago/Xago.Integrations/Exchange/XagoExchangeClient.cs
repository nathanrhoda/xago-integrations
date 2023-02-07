using Newtonsoft.Json;
using Xago.Integrations.Auth;

namespace Xago.Integrations
{
    public class XagoExchangeClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public static string ExchangeClient = "XagoExchange";

        public XagoExchangeClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> Transfer(HttpRequestMessage request)
        {
            var client = _httpClientFactory.CreateClient(ExchangeClient);
            var response = await client.SendAsync(request);

            var responseData = await response.Content.ReadAsStringAsync();
            
            if (string.IsNullOrEmpty(responseData))                
                return responseData;
            
            return JsonConvert.DeserializeObject(responseData).ToString();
        }
    }
}