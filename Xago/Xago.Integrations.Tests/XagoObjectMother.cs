using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Xago.Integrations.Auth;

namespace Xago.Integrations.Tests
{
    public static class XagoObjectMother
    {

        public  static HttpRequestMessage AuthRequest(IConfiguration _configuration)
        {
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
            var path = "v1/login";

            

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

        //public static HttpRequestMessage ExchangeRequest(IConfiguration configuration)
        //{
        //    var exchangeRequest = new XagoExchangeRequest
        //    {
        //        Source =
        //        {
                    
        //        }
        //    };
        //}
    }
}
