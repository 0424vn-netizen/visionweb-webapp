using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.VW.Entities
{
    public class StatementItem
    {
        public PdfStatementType StatementType { get; set; }

        public string ParameterContent { get; set; }
    }
}
