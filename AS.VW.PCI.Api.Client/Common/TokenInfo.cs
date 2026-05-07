namespace AS.VW.PCI.Api.Client.Common
{
    public class TokenInfo
    {
        public string AccessToken { get; set; }

        public string TokenType { get; set; }

        public int ExpireMinutes { get; set; }

    }
}
