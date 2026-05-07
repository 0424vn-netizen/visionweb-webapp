using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.WS.Mobile.Domain.Models
{
    public class Monthly
    {
        public List<MonthlyVolume> Items { get; set; }

        public Monthly()
        {
            Items = new List<MonthlyVolume>();
        }
    }

    public class Analysis
    {
        public List<VolumeAnalysis> Items { get; set; }

        public Analysis()
        {
            Items = new List<VolumeAnalysis>();
        }
    }

    public class Card
    {
        public List<CardVolume> Items { get; set; }

        public Card()
        {
            Items = new List<CardVolume>();
        }
    }

    public class ChartItem
    {
        public List<ChartVolume> Items { get; set; }

        public ChartItem()
        {
            Items = new List<ChartVolume>();
        }
    }

    public class ChartVolume
    {
        public string Months { get; set; }

        public decimal? SaleAmount { get; set; }

        public DateTime ReportDate { get; set; }
    }

    public class MonthlyVolume
    {
        public string Months { get; set; }

        public string Years { get; set; }

        public decimal Volume { get; set; }

        public long TransactionCount { get; set; }
    }

    public class CardVolume
    {
        public string CardType { get; set; }

        public decimal MTDSalesVolume { get; set; }

        public long MTDSalesTransaction { get; set; }

        public decimal YTDSalesVolume { get; set; }

        public long YTDSalesTransaction { get; set; }

        public decimal Last12MonthsSalesVolume { get; set; }

        public long Last12MonthsSalesTransaction { get; set; }
    }

    public class VolumeAnalysis
    {
        public string Title { get; set; }

        public decimal SaleAmount { get; set; }

        public long SaleCount { get; set; }

        public decimal ReturnAmount { get; set; }

        public decimal ReturnPercent { get; set; }

        public decimal RetrievalAmount { get; set; }

        public long RetrievalCount { get; set; }

        public decimal RetrievalPercent { get; set; }

        public decimal ChargeBackAmount { get; set; }

        public long ChargeBackCount { get; set; }

        public decimal ChargeBackPercent { get; set; }

        public decimal NetAmount { get; set; }

        public long KeyedCount { get; set; }

        public decimal KeyedPercent { get; set; }
    }
}
