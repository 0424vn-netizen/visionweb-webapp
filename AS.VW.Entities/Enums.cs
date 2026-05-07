using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.VW.Entities
{
    public enum StatementStatus
    {
        InQueue = 0,
        Success = 1,
        Processing = 2,
        Fail = 3
    }

    public enum PdfStatementType
    {
        DocServer,
        TSYS_TextFile
    }

    public enum PdfStatementResultType
    {
        NotSet,
        Path,
        Byte
    }

    public enum CustomViewMessageResourceType
    {
        CustomView = 1,
        Assignments = 2
    }
}
