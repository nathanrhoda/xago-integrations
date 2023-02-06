using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Xago.Integrations.Tests
{
    public class XagoAuthClientTests
    {
        protected IConfiguration Configuration;
        protected static IHttpClientFactory? _httpClientFactory;
        protected static IServiceCollection? _serviceCollection;
        

        public XagoAuthClientTests(string settingsFile = "appSettings.local.json")
        {
            _serviceCollection = new ServiceCollection();
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
        public async void GetToken_WhereValidCredentialsSupplied_ReturnsValidToken()
        {
            var xagoAuthClient = new XagoAuthClient(_httpClientFactory);            
            HttpRequestMessage request = XagoObjectMother.AuthRequest(Configuration);

            var response = await xagoAuthClient.GetToken(request);
            
            Assert.NotNull(response);
            Assert.True(response.tokenValue.Length > 0);
        }

    }
}