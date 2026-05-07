using AS.VW.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.VW.PDFStatementCreator
{
    public abstract partial class PdfStatementCreatorBase : IPdfStatementCreator
    {
        public static IPdfStatementCreator CreateNewInstance(PdfStatementType type, string culture, PdfStatementResultType forceReturnType = PdfStatementResultType.NotSet)
        {
            switch (type)
            {
                case PdfStatementType.DocServer:
                    return new DocServerPdfStatementCreator() { CurrentCulture = culture, ForceReturnType = forceReturnType };
                case PdfStatementType.TSYS_TextFile:
                    return new TsysTextPdfStatementCreator() { CurrentCulture = culture, ForceReturnType = forceReturnType };
                default:
                    return null;
            }
        }
    }
}
