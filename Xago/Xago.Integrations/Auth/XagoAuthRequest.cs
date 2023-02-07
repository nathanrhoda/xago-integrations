using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Xago.Integrations.Auth
{
    [Serializable]
    public class XagoAuthRequest
    {
        public XagoAuthRequest(string policyId, List<FieldProperty> fields, bool multiFactor)
        {
            this.PolicyId = policyId;
            this.Fields = fields;
            this.MultiFactor = multiFactor;
        }

        [JsonProperty("policyId")]
        public string PolicyId { get; private set; }

        [JsonProperty("fields")]
        public List<FieldProperty> Fields { get; private set; }

        [JsonProperty("mutliFactor")]
        public bool MultiFactor { get; private set; }
    }
}
