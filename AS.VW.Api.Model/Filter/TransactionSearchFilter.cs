using AS.VW.Api.Model.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AS.VW.Api.Model.Filter
{
    [DataContract]
    public class TransactionSearchFilter : GenericReportFilterNoViewLevel
    {
        [DataMember]
        public string First6CardNumber { get; set; }

        [DataMember]
        public string Last4CardNumber { get; set; }

        [DataMember]
        public string AuthorizationNumber { get; set; }

        [DataMember]
        public string TransactionOperator { get; set; }

        //[DataMember]
        public TransactionOperator TransactionOperatorValidate
        {
            get
            {
                if (string.IsNullOrEmpty(TransactionOperator))
                    return Filter.TransactionOperator.None;

                if (TransactionOperator == "Between")
                    return Filter.TransactionOperator.Between;
                if (TransactionOperator == "EqualTo")
                    return Filter.TransactionOperator.EqualTo;
                if (TransactionOperator == "GreaterThan")
                    return Filter.TransactionOperator.GreaterThan;
                if (TransactionOperator == "LessThan")
                    return Filter.TransactionOperator.LessThan;
                if (TransactionOperator == "PlusMinus5")
                    return Filter.TransactionOperator.PlusMinus5;
                return Filter.TransactionOperator.None;
            }
            set { TransactionOperator = value.ToString(); }
        }

        [DataMember]
        public decimal? TransactionAmountFrom { get; set; }

        [DataMember]
        public decimal? TransactionAmountTo { get; set; }

    }
}
