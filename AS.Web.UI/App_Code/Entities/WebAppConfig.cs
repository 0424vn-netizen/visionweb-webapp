using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AS.Web.UI.AppCode.Entities
{
    /// <summary>
    /// Summary description for WebAppConfig
    /// </summary>
    public class WebAppConfig
    {
        public TransactionHistoryGrid TransactionHistoryGrid { get; set; }
    }

    public class TransactionHistoryGrid
    {
        public bool EnhancePerformance { get; set; }
    }
}