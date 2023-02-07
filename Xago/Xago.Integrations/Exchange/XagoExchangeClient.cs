namespace Xago.Integrations
{
    public class XagoExchangeClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public static string AuthClient = "XagoExchange";

        public XagoExchangeClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public Task Transfer(HttpRequestMessage request)
        {
            throw new NotImplementedException();
        }
    }
}