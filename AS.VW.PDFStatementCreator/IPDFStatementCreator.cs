using AS.VW.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.VW.PDFStatementCreator
{
    public interface IPdfStatementCreator
    {
        string CurrentCulture { get; set; }

        PdfStatementResultType ForceReturnType { get; set; }

        PdfStatementCreatorResult Execute(object parameterContent
                                         ,int? asClientId = null, int? siteId = null
                                         , string userId = "", string userMode = ""
                                         , DateTime? reportDate = null);
    }
}
