using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Diagnostics;
using System.IO;
using Xago.Integrations.Auth;
using Xago.Integrations.Exchange;

namespace Xago.Integrations.Tests
{
    public static class XagoObjectMother
    {

        public  static HttpRequestMessage AuthRequest(IConfiguration _configuration)
        {
            var path = "v1/login";
            var apiKey = _configuration.GetSection("apiKey").Value;
            var policyId = _configuration.GetSection("policyId").Value;
            var multiFactor = Convert.ToBoolean(_configuration.GetSection("multiFactor").Value);
            var fieldsSection = _configuration.GetSection("fields");
            var children = fieldsSection.GetChildren();


            var fields = new List<FieldProperty>();
            foreach (var child in children)
            {
                var field = new FieldProperty
                    (
                        child.GetSection("fieldName").Value,
                        child.GetSection("fieldValue").Value
                    );

                fields.Add(field);
            }                        

            var authRequest = new XagoAuthRequest(policyId, fields, multiFactor);

            var stringData = JsonConvert.SerializeObject(authRequest);
            var requestContent = new StringContent(stringData, System.Text.Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, path)
            {
                Content = requestContent
            };
            request.Headers.Add("x-api-key", apiKey);
            return request;
        }

        public static HttpRequestMessage ExchangeRequest(IConfiguration _configuration, string token)
        {
            var path = "v1/transactions/Transfer";
            var exchangeRequest = new XagoExchangeRequest
            {
                Values = new List<ExchangeValue>
                {
                    new ExchangeValue
                    {
                        Source = new ExchangeSource
                        {
                            ExternalId = _configuration.GetSection("sourceExternalId").Value,
                            MobileNumber = _configuration.GetSection("sourceMobileNumber").Value,
                        },
                        Destination = new ExchangeDestination
                        {
                            ExternalId = _configuration.GetSection("destinationExternalId").Value,
                        },
                        Amount = _configuration.GetSection("amount").Value,
                        CurrencyCode = _configuration.GetSection("currencyCode").Value,
                        AccountNumber = _configuration.GetSection("accountNumber").Value,
                        Reason = _configuration.GetSection("reason").Value,
                        Reference = _configuration.GetSection("reference").Value,
                    }                    
                }
            };

            var stringData = JsonConvert.SerializeObject(exchangeRequest);
            var requestContent = new StringContent(stringData, System.Text.Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, path)
            {
                Content = requestContent
            };
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            return request;
        }
    }
}
