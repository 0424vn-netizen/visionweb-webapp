using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace AS.WS.Mobile.Domain.Models
{
    public class Statements
    {
        public string Id { get; set; }
        public List<Statement> Items { get; set; }

        public Statements()
        {
            Items = new List<Statement>();
        }
    }
    public class ChainStatement
    {
        public DateTime ReportDate { get; set; }
        public string FileName { get; set; }
        public string Processor { get; set; }
        public string DocID { get; set; }

        public List<ChainStatement> Items { get; set; }

        public ChainStatement()
        {
            Items = new List<ChainStatement>();
        }
    }

    public class Statement
    {
        public long Id { get; set; }
        public DateTime ReportDate { get; set; }
        public long DateTick { get { return ReportDate.Ticks; } }
        public int FileIndex { get; set; }
        public string DateMonth
        {
            get
            {
                if (FileIndex > 0)
                    return string.Format("{0} - {1}", ReportDate.ToString("MMM yyyy"), FileIndex);

                return ReportDate.ToString("MMM yyyy");
            }
        }
        public decimal TotalAmount { get; set; }
        public decimal NetDeposit { get; set; }
        public StatementDetail Items { get; set; }
        public string Mid { get; set; }
        public string DocID { get; set; }
        public string FileName { get; set; }
        public bool HistoricalStatement { get; set; }
        public string BackEndProcessor { get; set; }

        public Statement()
        {
            Items = new StatementDetail();
        }
    }

    public class StatementDetail
    {
        public string Name { get; set; }

        public decimal Amount { get; set; }

        public DepositSummary DepositSummary { get; set; }

        public CardSummary CardSummary { get; set; }

        public SettlementSummary SettlementSummary { get; set; }

        public SurchargeSummary SurchargeSummary { get; set; }

        public OtherFeesSummary OtherFeesSummary { get; set; }

        public DepositWrfcSummary DepositWRFCSummary { get; set; }

        public PlanSummary PlanSummary { get; set; }

        public ChargebackSummary ChargebackSummary { get; set; }

        public AdjustmentWrfcSummary AdjustmentWRFCSummary { get; set; }

        public WrfcFeesSummary WRFCFeesSummary { get; set; }

        public MoreSummary MoreSummary { get; set; }

        public MoreWrfcSummary MoreWRFCSummary { get; set; }

        public StatementDetail()
        {
            DepositSummary = new DepositSummary();
            CardSummary = new CardSummary();
            SettlementSummary = new SettlementSummary();

            SurchargeSummary = new SurchargeSummary();
            OtherFeesSummary = new OtherFeesSummary();

            DepositWRFCSummary = new DepositWrfcSummary();
            PlanSummary = new PlanSummary();
            ChargebackSummary = new ChargebackSummary();
            AdjustmentWRFCSummary = new AdjustmentWrfcSummary();
            WRFCFeesSummary = new WrfcFeesSummary();
            MoreSummary = new MoreSummary();
            MoreWRFCSummary = new MoreWrfcSummary();
        }
    }

    public class StatementLineItem
    {
        public string Title { get; set; }

        public decimal Amount { get; set; }
    }

    public static class StatementResult
    {
        public static List<Statement> ListStatementResult { get; set; }
    }

    public class DepositSummary
    {
        public decimal Sales { get; set; }
        public long SaleCount { get; set; }
        public decimal Credits { get; set; }

        public decimal AdjustedTotal { get; set; }

        public decimal SubTotal { get; set; }
    }

    public class DepositItemSummary
    {
        public List<DepositItem> Items { get; set; }
        public DepositItemSummary()
        {
            Items = new List<DepositItem>();
        }
    }

    public class DepositItem
    {
        public long SaleCount { get; set; }
        public decimal SaleAmount { get; set; }
        public long ReturnCount { get; set; }
        public decimal ReturnAmount { get; set; }
        public long DebitAdjustCount { get; set; }
        public decimal DebitAdjustAmount { get; set; }
        public long CreditAdjCount { get; set; }
        public decimal CreditAdjustAMount { get; set; }
    }

    public class CardSummary
    {
        public decimal MasterCard { get; set; }

        public decimal Discover { get; set; }

        public decimal Visa { get; set; }

        public decimal AMEX { get; set; }

        public decimal Diners { get; set; }

        public decimal DEBIT { get; set; }

        public decimal Others { get; set; }
    }

    public class SettlementSummary
    {
        public decimal SettlementDiscountAmount { get; set; }
    }

    public class SettlementDetailsSummary
    {
        //TK: 42609 - Update Settlement/Discount and Other Fees Tabs on VW Mobile
        public long TotalItems { get; set; }
        public decimal TotalAmount { get; set; }
        public List<SettlementDetails> Items { get; set; }
        public SettlementDetailsSummary()
        {
            Items = new List<SettlementDetails>();
        }
    }

    public class SettlementDetails
    {
        public string Description { get; set; }
        public decimal FeeAmount { get; set; }
        public decimal Amount { get; set; }
        public decimal Interchange { get; set; }
        public long Items { get; set; }
    }


    public class SurchargeSummary
    {
        public decimal SurchargeAmount { get; set; }
    }

    public class SurchargeDetailsSummary
    {
        public long TotalItems { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal SurchargeAmount { get; set; }
        public List<SurchargeDetails> Items { get; set; }
        public SurchargeDetailsSummary()
        {
            Items = new List<SurchargeDetails>();
        }
    }

    public class SurchargeDetails
    {
        public string Description { get; set; }
        public decimal SurchargeAmount { get; set; }
        public long Items { get; set; }
    }


    public class OtherFeesSummary
    {
        public decimal OtherFees { get; set; }
    }

    public class OtherFeesDetailsSummary
    {
        //TK: 42609 - Update Settlement/Discount and Other Fees Tabs on VW Mobile
        public decimal TotalAmount { get; set; }
        public List<OtherFeesDetails> Items { get; set; }
        public OtherFeesDetailsSummary()
        {
            Items = new List<OtherFeesDetails>();
        }
    }

    public class OtherFeesDetails
    {
        public string CardTypeCode { get; set; }
        public string ShortDescription { get; set; }
        public decimal Amount { get; set; }
        public long Tickets { get; set; }
    }

    public class DepositWrfcSummary
    {
        public decimal Sales { get; set; }
        public long SaleCount { get; set; }
        public decimal Credits { get; set; }
        public decimal NetSales { get; set; }
    }

    public class PlanSummary
    {
        public decimal Sales { get; set; }
        public long SaleCount { get; set; }
        public decimal Credits { get; set; }
        public long CreditCount { get; set; }
        public decimal NetSales { get; set; }
    }

    public class AdjustmentWrfcSummary
    {
        public decimal Sales { get; set; }
        public long SaleCount { get; set; }
        public decimal Credits { get; set; }

        public decimal NetSales { get; set; }
    }

    public class ChargebackSummary
    {
        public decimal Sales { get; set; }
        public long SaleCount { get; set; }
        public decimal Credits { get; set; }

        public decimal NetSales { get; set; }
    }

    public class WrfcFeesSummary
    {
        public decimal OtherFees { get; set; }
    }

    public class MoreSummary
    {
        public decimal TotalAmount { get; set; }

        public decimal MinBillAdjustment { get; set; }
    }

    public class MoreWrfcSummary
    {
        public decimal DiscountPaid { get; set; }

        public decimal NetDiscountDue { get; set; }

        public decimal FeesDue { get; set; }

        public decimal FeePaid { get; set; }

        public decimal NetFeesDue { get; set; }

        public decimal AmountDeducted { get; set; }
    }

    public class MonthlyMessages
    {
        public List<MessagesItem> Messages { get; set; }
    }

    public class MessagesItem
    {
        public string Messages { get; set; }
    }
}