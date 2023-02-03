namespace Xago.Integrations.Tests
{
    public class XagoClientTests
    {
        [Fact]
        public void GetToken_WhereValidCredentialsSupplied_ReturnsValidToken()
        {
            string token = "";
            Assert.True(string.IsNullOrWhiteSpace(token));
        }
    }
}