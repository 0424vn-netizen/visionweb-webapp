using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AS.VW.Api.Model.Filter
{
    public enum HierarchyFilterMode
    {
        None,
        //Common
        Group,
        Association,
        MasterSalesAgent,
        CorporateName,
        MerchantName,
        MerchantNumber,
        //Omaha platform
        Sys,
        SysPrin,
        SysPrinAgent,
        SalesAgent,
        HeadquarterMerchantNumber,
        //North platform
        Bank,
        Agent,
        Corp,
        NorthChain,
        NorthSalesAgent,
        //Memphis
        MasterChain,
        MemphisChain,
        SalesmanNo,
        //Others
        MccSic,
        FeeClass,
        Misc1,
        Misc2,
        Last6MerchantNumber,
        Chain,
        Asso,
        Grp,
        MpsAgent,
        MpsChain,
        LAST6MERCHNUMBER
    }
}
