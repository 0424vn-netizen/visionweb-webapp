using System;
using System.Collections.Generic;

namespace AS.WS.Mobile.Domain.Models
{
    public class Deposits
    {
        public List<Deposit> Items { get; set; }

        public Deposits()
        {
            Items = new List<Deposit>();
        }
    }

    public class Deposit
    {
        public long ID { get; set; }

        public DateTime Date { get; set; }

        public string Mid { get; set; }

        public decimal Amount { get; set; }

        public DepositDetail Details { get; set; }
    }

    public class DepositDetail
    {
        public DebitDeposit DebitDeposit { get; set; }
        public List<TransactionTypes> ListTransactionTypes { get; set; }
        public DepositDetail()
        {
            DebitDeposit = new DebitDeposit();
            ListTransactionTypes = new List<TransactionTypes>();
        }
    }

    public class DebitDeposit
    {
        public decimal ReturnAmount { get; set; }
        public decimal SaleAmount { get; set; }
        public long SaleCount { get; set; }
        public long ReturnCount { get; set; }
    }

    public class TransactionTypes
    {
        public string TransactionType { get; set; }
        public decimal DepositAmount { get; set; }
        public long DepositCount { get; set; }
    }
}