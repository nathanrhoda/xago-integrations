using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http;
using System.Security.Authentication.ExtendedProtection;

namespace Xago.Integrations.Tests
{
    public class XagoClientTests
    {
        protected IConfiguration Configuration;
        protected static IHttpClientFactory? _httpClientFactory;
        protected static IServiceCollection? _serviceCollection;
        private readonly string AuthClient = "XagoAuth";

        public XagoClientTests(string settingsFile = "appSettings.local.json")
        {
            _serviceCollection = new ServiceCollection();
            _serviceCollection.AddHttpClient(AuthClient, client =>
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
            HttpRequestMessage request = XagoObjectMother.AuthRequest(Configuration);

            var client = _httpClientFactory.CreateClient(AuthClient);
            var response = await client.SendAsync(request);
            var responseData = await response.Content.ReadAsStringAsync();
            var xagoAuthResponse = JsonConvert.DeserializeObject<XagoAuthResponse>(responseData);
            Assert.NotNull(xagoAuthResponse);
            Assert.True(xagoAuthResponse.tokenValue.Length > 0);
        }

        
    }
}