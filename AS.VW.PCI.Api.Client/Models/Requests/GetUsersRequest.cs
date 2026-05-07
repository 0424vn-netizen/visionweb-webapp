namespace AS.VW.PCI.Api.Client.Models.Requests
{
    public class GetUsersRequest : AuthTokenRequest
    {
        public int ASClient { get; set; }

        public string UserName { get; set; }
    }
}
