namespace Xago.Integrations.Auth
{
    public class FieldProperty
    {
        public FieldProperty(string fieldName, string fieldValue)
        {
            this.fieldName = fieldName;
            this.fieldValue = fieldValue;
        }

        public string fieldName { get; }
        public string fieldValue { get; }
    }
}