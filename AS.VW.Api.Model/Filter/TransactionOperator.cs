using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AS.VW.Api.Model.Filter
{
    public enum TransactionOperator
    {
        None = 0,
        EqualTo,
        Between,
        GreaterThan,
        LessThan,
        PlusMinus5 
    }
}
