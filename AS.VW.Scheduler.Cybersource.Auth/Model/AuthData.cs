using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.VW.Scheduler.Cybersource.Auth.Model
{
    public class AuthData
    {
        public string SearchId { get; set; }
        public bool Save { get; set; }
        public string Name { get; set; }
        public string Query { get; set; }
        public int Count { get; set; }
        public int TotalCount { get; set; }
        public int Limit { get; set; }
        public int Offset { get; set; }
        public string Sort { get; set; }
        public string Timezone { get; set; }
        public DateTime SubmitTimeUtc { get; set; }

        [JsonProperty("_embedded")]
        public Transactions Transactions { get; set; } = new Transactions() { TransactionSummaries = new List<Transactionsummary>() };
    }

    public class Transactions
    {
        [JsonProperty("transactionSummaries")] 
        public List<Transactionsummary> TransactionSummaries { get; set; }
    }

    public class TransactionDetail
    {
        public string ReconciliationId { get; set; }
        public string Id { get; set; }
        public DateTime? SubmitTimeUtc { get; set; }
        public string MerchantId { get; set; }
        public ApplicationInformation ApplicationInformation { get; set; }
        public ClientReferenceInformation ClientReferenceInformation { get; set; }        
        public MerchantInformation MerchantInformation { get; set; }
        public OrderInformation OrderInformation { get; set; }
        public PaymentInformation PaymentInformation { get; set; }
        public ProcessingInformation ProcessingInformation { get; set; }
        public ProcessorInformation ProcessorInformation { get; set; }
        public PointofSaleInformation PointOfSaleInformation { get; set; }
    }
    public class Transactionsummary
    {
        public string Id { get; set; }
        public DateTime? SubmitTimeUtc { get; set; }
        public string MerchantId { get; set; }
        public PaymentInformation PaymentInformation { get; set; }
    }

    public class ApplicationInformation
    {
        public Application[] Applications { get; set; }
        public string ReasonCode { get; set; }
        public string rCode { get; set; }
        public string rFlag { get; set; }
    }

    public class Application
    {
        public string Name { get; set; }
        public string ReasonCode { get; set; }
        public string rCode { get; set; }
        public string rFlag { get; set; }
        public string ReconciliationId { get; set; }
        public string rMessage { get; set; }
        public string ReturnCode { get; set; }
    }

    public class ClientReferenceInformation
    {
        public string Code { get; set; }
        public string ApplicationName { get; set; }
    }

    public class MerchantInformation
    {
    }

    public class OrderInformation
    {
        public BillTo BillTo { get; set; }
        public ShipTo ShipTo { get; set; }
        public AmountDetails AmountDetails { get; set; }
    }

    public class BillTo
    {
        public string Address1 { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class ShipTo
    {
        public string Address1 { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class AmountDetails
    {
        public string TotalAmount { get; set; }
        public string AuthorizedAmount { get; set; }
        public string Currency { get; set; }
    }

    public class PaymentInformation
    {
        public PaymentType PaymentType { get; set; }
        public Customer Customer { get; set; }
        public Card Card { get; set; }
    }

    public class PaymentType
    {
        public string Type { get; set; }
        public string Method { get; set; }
    }

    public class Customer
    {
        public string CustomerId { get; set; }
    }

    public class Card
    {
        public string Suffix { get; set; }
        public string Prefix { get; set; }
        public string Type { get; set; }
        public string ExpirationMonth { get; set; }
        public string ExpirationYear { get; set; }
    }

    public class ProcessingInformation
    {
        public string CommerceIndicator { get; set; }
        public string CommerceIndicatorLabel { get; set; }
        public AuthorizationOptions AuthorizationOptions { get; set; }
    }

    public class AuthorizationOptions
    {
        public string AuthIndicator { get; set; }
    }

    public class ProcessorInformation
    {
        public Processor Processor { get; set; }
        public Avs Avs { get; set; }
        public string NetworkTransactionId { get; set; }
        public string ApprovalCode { get; set; }
        public string EventStatus { get; set; }
        public string TransactionId { get; set; }
        public CardVerification CardVerification { get; set; }
        public AchVerification AchVerification { get; set; }
        public string ResponseCode { get; set; }
    }
    public class CardVerification 
    { 
        public string ResultCode { get; set; }
        public string ResultCodeRaw { get; set; }
    }
    public class AchVerification
    {
        public string ResultCode { get; set; }
        public string ResultCodeRaw { get; set; }
    }

    public class Processor
    {
        public string Name { get; set; }
    }
    public class Avs
    {
        public string Code { get; set; }
        public string CodeRaw { get; set; }
    }

    public class PointofSaleInformation
    {
        public string TerminalId { get; set; }
        public Emv Emv { get; set; }
        public string EntryMode { get; set; }
    }

    public class Emv
    {
    }
}
