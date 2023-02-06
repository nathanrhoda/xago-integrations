namespace Xago.Integrations
{
    public class XagoAuthRequest
    {        
        public XagoAuthRequest(string policyId, List<FieldProperty> fields, bool multiFactor)
        {
            this.policyId = policyId;
            this.fields = fields;
            this.multiFactor = multiFactor;
        }
     
        public string policyId { get; private set; }
        public List<FieldProperty> fields { get; private set; }
        public bool multiFactor { get; private set; }
    }
}
