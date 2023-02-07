using Newtonsoft.Json;
using System.Text.Json.Nodes;

namespace Xago.Integrations.Exchange
{
    [Serializable]
    public class XagoExchangeRequest
    {
        [JsonProperty("values")]
        public List<ExchangeValue> Values { get; set; }
        
    }

    [Serializable]
    public class ExchangeValue
    {
        [JsonProperty("source")]
        public ExchangeSource Source { get; set; }

        [JsonProperty("destination")]
        public ExchangeDestination Destination { get; set; }

        [JsonProperty("amount")]
        public string Amount { get; set; }

        [JsonProperty("currencyCode")]
        public string CurrencyCode { get; set; }

        [JsonProperty("accountNumber")]
        public string AccountNumber { get; set; }

        [JsonProperty("reason")]
        public string Reason { get; set; }

        [JsonProperty("reference")]
        public string Reference { get; set; }
    }

    [Serializable]
    public class ExchangeSource
    {
        [JsonProperty("externalId")]
        public string ExternalId { get; set; }

        [JsonProperty("mobileNumber")]
        public string MobileNumber { get; set; }
    }

    [Serializable]
    public class ExchangeDestination
    {
        [JsonProperty("externalId")]
        public string ExternalId { get; set; }
    }
}