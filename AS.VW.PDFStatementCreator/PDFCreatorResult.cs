using AS.VW.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.VW.PDFStatementCreator
{
    public class PdfStatementCreatorResult
    {
        public PdfStatementResultType ResultType { get; set; }

        public object Content { get; set; }

        public PdfStatementCreatorResult() { }

        public PdfStatementCreatorResult(PdfStatementResultType resultType, PdfStatementResultType forceResultType, object content)
        {
            Content = content;
            ResultType = resultType;
            switch (resultType)
            { 
                case PdfStatementResultType.Byte:
                    if (forceResultType == PdfStatementResultType.Path)
                    {
                        ResultType = PdfStatementResultType.Path;
                        Content = FileHandler.SaveFile(content as byte[]);
                    }
                    break;
                case PdfStatementResultType.Path:
                    if (forceResultType == PdfStatementResultType.Byte)
                    {
                        ResultType = PdfStatementResultType.Byte;
                        Content = FileHandler.ReadFile(content as string);
                    }
                    break;
            }
        }
    }
}
