using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xago.Integrations.Auth;

namespace Xago.Integrations.Tests
{
    public class XagoExchangeClientTests
    {
        protected IConfiguration Configuration;
        protected static IHttpClientFactory? _httpClientFactory;
        protected static IServiceCollection? _serviceCollection;


        public XagoExchangeClientTests(string settingsFile = "appSettings.local.json")
        {
            _serviceCollection = new ServiceCollection();
            _serviceCollection.AddHttpClient(XagoExchangeClient.ExchangeClient, client =>
            {
                client.BaseAddress = new Uri(Configuration.GetSection("exchangeUrl").Value);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            });
            _serviceCollection.AddHttpClient(XagoAuthClient.AuthClient, client =>
            {
                client.BaseAddress = new Uri(Configuration.GetSection("authUrl").Value);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            });

            var builder = new ConfigurationBuilder()
                .AddJsonFile(settingsFile);
            Configuration = builder.Build();

            var serviceProvider = _serviceCollection.BuildServiceProvider();
            _httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
        }

        [Fact]
        public async void Transfer_WhereValidDetailsSupplied_ReturnsTransactionId()
        {
            var token = await GetToken();
            var client = new XagoExchangeClient(_httpClientFactory);
            HttpRequestMessage request = XagoObjectMother.ExchangeRequest(Configuration, token);

            var response = await client.Transfer(request);

            Assert.NotNull(response);
            Assert.True(response.Length > 3);
        }

        private async Task<string> GetToken()
        {
            var xagoAuthClient = new XagoAuthClient(_httpClientFactory);
            HttpRequestMessage request = XagoObjectMother.AuthRequest(Configuration);

            var response = await xagoAuthClient.GetToken(request);
            return response.tokenValue;
        }
        
    }
}
