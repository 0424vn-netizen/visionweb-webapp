namespace AS.VW.Scheduler.Cybersource.Auth.Model
{
    public class TransactionDetailCache
    {
        public string RawResponse { get; set; }

        public TransactionDetail ParsedDetail { get; set; }
    }
}
