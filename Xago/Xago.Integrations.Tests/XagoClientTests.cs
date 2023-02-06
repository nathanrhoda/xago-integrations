using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http;

namespace Xago.Integrations.Tests
{
    public class XagoClientTests
    {
        protected IConfiguration Configuration;
        private static HttpClient _httpClient;

        public XagoClientTests(string settingsFile = "appSettings.local.json")
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile(settingsFile);
            Configuration = builder.Build();
            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri(uriString: Configuration.GetSection("authUrl").Value)
            };            
        }

        [Fact]
        public async void GetToken_WhereValidCredentialsSupplied_ReturnsValidToken()
        {
            HttpRequestMessage request = AuthRequest();

            var response = await _httpClient.SendAsync(request);
            var responseData = await response.Content.ReadAsStringAsync();
            var xagoAuthResponse = JsonConvert.DeserializeObject<XagoAuthResponse>(responseData);
            Assert.NotNull(xagoAuthResponse);
            Assert.True(xagoAuthResponse.tokenValue.Length > 0);
        }

        private HttpRequestMessage AuthRequest()
        {
            var policyId = Configuration.GetSection("policyId").Value;
            var multiFactor = Convert.ToBoolean(Configuration.GetSection("multiFactor").Value);
            var fieldsSection = Configuration.GetSection("fields");
            var children = fieldsSection.GetChildren();


            var fields = new List<FieldProperty>();
            foreach (var child in children)
            {
                var field = new FieldProperty
                    (
                        fieldName: child.GetSection("fieldName").Value,
                        fieldValue: child.GetSection("fieldValue").Value
                    );

                fields.Add(field);
            }

            var path = "v1/login";

            var apiKey = Configuration.GetSection("apiKey").Value;

            var xagoRequest = new XagoAuthRequest(policyId, fields, multiFactor);

            var stringData = JsonConvert.SerializeObject(xagoRequest);
            var requestContent = new StringContent(stringData, System.Text.Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, path)
            {
                Content = requestContent
            };
            request.Headers.Add("x-api-key", apiKey);
            return request;
        }
    }
}